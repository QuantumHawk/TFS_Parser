using System.Globalization;
using Npgsql;
using Pgvector;
using OpenAI;
using OpenAI.Embeddings;

namespace TFS.Services;

public class Calculate
{
    private const string getPrecedentsParamAlt = @"SELECT ""ID"", ""B"", ""V"", ""T"" FROM ""PrecedentsParamAlt""";
    private const string getParamAlt = @"SELECT ""ID"", ""B"", ""V"", ""T"" FROM ""ROOTMAINLISTTFSTFEPARAMSParamAlt""";
    
    public async Task Test()
    {
 
        
        //select from db to get alternatives
        // conn string
        const string cs = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";

// 1) Регистрируем pgvector на DataSource
        var dsb = new NpgsqlDataSourceBuilder(cs);
        dsb.UseVector(); // ВАЖНО
        await using var dataSource = dsb.Build();

// 2) Читаем имена из вашей таблицы ИЗ dataSource
        var names = new List<string>();
        try
        {
            await using (var conn = dataSource.OpenConnection())
            {
                //const string sql = @"SELECT ""NAME"" FROM ""ROOTMAINLISTTFSTFEPARAMSParamAlt""";
                const string sql = @"SELECT ""NAME"" FROM ""PrecedentsParamAlt""";
                await using var cmd1 = new NpgsqlCommand(sql, conn);
                await using var reader = await cmd1.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                    names.Add(reader.GetString(0));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


// 3) Парсим строки -> (Group, Role, Full)
        var items = names.Select(line =>
        {
            var lb = line.IndexOf('[');
            var rb = line.IndexOf(']');
            if (lb < 0 || rb <= lb) throw new Exception($"Bad line: {line}");

            var group = line.Substring(lb + 1, rb - lb - 1).Trim();
            var role = line.Substring(rb + 1).Trim();
            if (role.Length == 0) role = null;

            var full = role is null ? group : $"{group}. Роль: {role}";
            return new { Group = group, Role = role, Full = full };
        }).ToList();

// 4) Считаем эмбеддинги (async, без .Result)
//    По желанию делайте батчом: GenerateEmbeddingsAsync(items.Select(i=>i.Full))
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var embedClient = new EmbeddingClient("text-embedding-3-small", apiKey);

        var vectors = new List<float[]>(items.Count);
        foreach (var it in items)
        {
            var resp = embedClient.GenerateEmbedding(it.Full);
            vectors.Add(resp.Value.ToFloats().ToArray()); // 1536 значений
        }

// 5) Вставляем в БД (через dataSource, один cmd, параметры реиспользуем)
        const string insertSql = @"
    INSERT INTO domain_objects(id, grp, role, full_text, embedding)
    VALUES (@id, @grp, @role, @full, @emb);";

         using (var conn =  dataSource.OpenConnection())
         using (var tx =  conn.BeginTransaction())
         using (var cmd = new NpgsqlCommand(insertSql, conn, tx))
        {
            // создаём параметры один раз с "пустыми" значениями
            cmd.Parameters.AddWithValue("id",   Guid.Empty);
            cmd.Parameters.AddWithValue("grp",  "");
            cmd.Parameters.AddWithValue("role", "");
            cmd.Parameters.AddWithValue("full", "");
            cmd.Parameters.AddWithValue("emb",  new Vector(new float[1536])); // dummy вектор

            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                var vec = new Vector(vectors[i]);

                cmd.Parameters["id"].Value   = Guid.NewGuid();
                cmd.Parameters["grp"].Value  = it.Group;
                cmd.Parameters["role"].Value = it.Role ?? "";
                cmd.Parameters["full"].Value = it.Full;
                cmd.Parameters["emb"].Value  = vec;

                cmd.ExecuteNonQuery();
            }

            await tx.CommitAsync();
        }
    }


    public async Task<Dictionary<Guid,List<float>>> Test2(string sql)
    {
        // 1) DataSource с pgvector-хэндлером
        var cs = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";
        var dsb = new NpgsqlDataSourceBuilder(cs);
        dsb.UseVector();
        await using var dataSource = dsb.Build();
        
        var TFEs = new Dictionary<Guid,List<float>>();
        try
        {
            await using (var conn3 = dataSource.OpenConnection())
            {

                await using var cmd1 = new NpgsqlCommand(sql, conn3);
                await using var reader = await cmd1.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = reader.GetString(0);
                    var b = reader.GetString(1);
                    var v = reader.GetString(2);
                    var t = reader.GetString(3);

                    var list = new List<float>();
                    list.Add(float.Parse(v, CultureInfo.InvariantCulture));
                    list.Add(float.Parse(t, CultureInfo.InvariantCulture));
                    list.Add(float.Parse(b, CultureInfo.InvariantCulture));

                    TFEs.Add(Guid.Parse(id), list);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return TFEs;
    }

    public async Task Search()
    {
        // 1) DataSource с pgvector-хэндлером
        var cs = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";
        var dsb = new NpgsqlDataSourceBuilder(cs);
        dsb.UseVector();
        await using var dataSource = dsb.Build();
        await using var conn =  dataSource.OpenConnection();
        
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var embedClient = new EmbeddingClient("text-embedding-3-small", apiKey);

        try
        {
            var emb =  embedClient.GenerateEmbedding(
                "Диагностика неисправности в офисе, системный");
            var q = new Vector(emb.Value.ToFloats().ToArray());
            // Поиск k-NN: роль — через COALESCE, плюс расстояние
            const string sql = @"
            SELECT id,
                   grp,
                   COALESCE(role, '') AS role,
                   full_text,
                   embedding <-> @q AS distance
            FROM domain_objects
            ORDER BY distance
            LIMIT 5;";

            await using var searchCmd = new NpgsqlCommand(sql, conn);
            searchCmd.Parameters.AddWithValue("q", q);

            var result = new Dictionary<Guid,List<float>>();
            using var r = searchCmd.ExecuteReader();
            while (await r.ReadAsync())
            {
                var id   = r.GetGuid(0);
                var grp  = r.GetString(1);
                var role = r.GetString(2);   
                var text = r.GetString(3);
                var dist = r.GetFloat(4);

                Console.WriteLine($"{id} | {grp} | {role} | dist={dist:0.000}");
            }
            
            var dicts = await Task.WhenAll(
                Test2(getPrecedentsParamAlt),
                Test2(getParamAlt)
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}