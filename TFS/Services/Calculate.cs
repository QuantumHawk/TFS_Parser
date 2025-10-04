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
    //private const string connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";
    private const string connectionString = "Host=localhost;Database=test;Username=postgres;Password=admin;";
    private const string insertSql = @" INSERT INTO domain_objects(id, grp, role, full_text, embedding) VALUES (@id, @grp, @role, @full, @emb);";
    const string sql = @"
            SELECT id,
                   grp,
                   COALESCE(role, '') AS role,
                   full_text,
                   embedding <-> @q AS distance,
                   id_tfc
            FROM domain_objects
            ORDER BY distance
            LIMIT 4;";
    
    
    //TODO:
    //1. move migrations to this project - remove from TFS_Parser
    //2. setup vector for domain table - write down all commands
    //3. add method Create TFS with new changed param alt
    public async Task CalculateEmbding()
    {
        // 1) Регистрируем pgvector на DataSource
        var dsb = new NpgsqlDataSourceBuilder(connectionString);
        dsb.UseVector(); // ВАЖНО
        await using var dataSource = dsb.Build();

        // 2) Читаем имена из вашей таблицы ИЗ dataSource
        var names = new List<string>();
        try
        {
            await using (var conn = dataSource.OpenConnection())
            {
                //1 сначала считаем для парам альтернатив, которые есть в самой структуре
                //TODO: 
                const string sql = @"SELECT ""NAME"" FROM ""ROOTMAINLISTTFSTFEPARAMSParamAlt"" where ""ID_DB"" != 11";
                //2 потом считаем для добавленных вручную прецедентов
                //const string sql = @"SELECT ""NAME"" FROM ""PrecedentsParamAlt""";
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

    //TODO: переделать в джоин 2 таблиц и вытащить значения одним запросом
    public async Task<Dictionary<Guid,List<float>>> GetTableDataBy(string query)
    {
        // 1) DataSource с pgvector-хэндлером
        var dsb = new NpgsqlDataSourceBuilder(connectionString);
        await using var dataSource = dsb.Build();
        
        var TFEs = new Dictionary<Guid,List<float>>();
        try
        {
            await using (var conn = dataSource.OpenConnection())
            {

                await using var cmd = new NpgsqlCommand(query, conn);
                using var reader =  cmd.ExecuteReader();
                while ( reader.Read())
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
    
    public async Task FindSolution()
    {
        // 1) DataSource с pgvector-хэндлером
        var dsb = new NpgsqlDataSourceBuilder(connectionString);
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

            await using var searchCmd = new NpgsqlCommand(sql, conn);
            searchCmd.Parameters.AddWithValue("q", q);

            var result = new Dictionary<Guid,float>();
            
            var precedents = await GetTableDataBy(getPrecedentsParamAlt);
            var original = await GetTableDataBy(getParamAlt); 
            var compareID = Guid.NewGuid();
            var comparedTFE = "";
            var compareB = 0.0;
            var compareT = 0.0;
            var compareV = 0.0;
            var tfe_name = "";
            var empty = true;
            var readyToCompare = false;
            
            using var r = searchCmd.ExecuteReader();
            while (await r.ReadAsync())
            {
                var id   = r.GetGuid(0);
                var grp  = r.GetString(1);
                var role = r.GetString(2);   
                var text = r.GetString(3);
                var dist = r.GetFloat(4);
                var id_tfc = r.GetGuid(5);

                var t = $"{id_tfc} | {grp} | {role} | {text} | dist={dist:0.000}";
                Console.WriteLine(t);
                
                precedents.TryGetValue(id_tfc, out var pr);
                original.TryGetValue(id_tfc, out var or);
                
                //сравнить BTV между собой и выбрать лучшую ТФС из прецедентов
                if (pr is not null)
                {
                    if (empty)
                    {
                        compareID = id_tfc;
                        comparedTFE = t;
                        tfe_name = grp + " " + role;
                        compareV = pr[0];
                        compareT = pr[1];
                        compareB = pr[2];   
                        empty =  false;
                        readyToCompare = true;
                    }
                    if(readyToCompare)
                    {
                        if (compareV > pr[0] && compareT > pr[1] && compareB < pr[2])
                        {
                            compareID = id_tfc;
                            comparedTFE = t;
                            tfe_name = grp + " " + role;
                            compareV = pr[0];
                            compareT = pr[1];
                            compareB = pr[2]; 
                        }
                    } 
                }

                if (or is not null)
                {
                    if (empty)
                    {
                        compareID = id_tfc;
                        comparedTFE = t;
                        tfe_name = grp + " " + role;
                        compareV = or[0];
                        compareT = or[1];
                        compareB = or[2];
                        empty =  false;
                        readyToCompare = true;
                    }
                    if(readyToCompare)
                    {
                        if (compareV < or[0] && compareT < or[1] && compareB > or[2])
                        {
                            compareID = id_tfc;
                            comparedTFE = t;
                            tfe_name = grp + " " + role;
                            compareV = or[0];
                            compareT = or[1];
                            compareB = or[2]; 
                        }
                    } 
                }
            }
            
            var p = $"Best result {comparedTFE} | {compareV} | {compareT} | {compareB}";
            Console.WriteLine(p);
            
            //logic create new TFS with optimised TFE
            var res = await UpdateTfsAsync(compareID,tfe_name, compareV, compareT, compareB);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public async Task<int> UpdateTfsAsync(Guid id, string tfe_text, double v, double t, double b)
    {
        const string sql = @"
        UPDATE ""ROOTMAINLISTTFSTFEPARAMSParamAlt""
        SET    ""NAME"" = @name,
               ""B""    = @b,
               ""V""    = @v,
               ""T""    = @t
        WHERE  ""ID""   = @id;";

        const string cs = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";

        // DataSource без UseVector (векторы тут не используются)
        var dsb = new NpgsqlDataSourceBuilder(cs);
        await using var dataSource = dsb.Build();

        using var conn = dataSource.OpenConnection();
        using var cmd  = new NpgsqlCommand(sql, conn);

        // параметры создаём один раз (AddWithValue — как вы просили)
        cmd.Parameters.AddWithValue("name", tfe_text ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("b", b);   // double → double precision
        cmd.Parameters.AddWithValue("v", v);
        cmd.Parameters.AddWithValue("t", t);
        cmd.Parameters.AddWithValue("id","a0297d99-2cd2-47a8-a1fe-40ce177887b7");

        // выполняем запрос
        var affected = await cmd.ExecuteNonQueryAsync();
        return affected;
    }

    //TODO
    public async Task<int> CreateNewTfsAsync(Guid id, string tfe_text, double v, double t, double b)
    {
        return await CreateNewTfsAsync(id, tfe_text, v, t, b);
    }
}