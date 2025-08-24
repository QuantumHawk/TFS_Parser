using Npgsql;
using Pgvector;
using OpenAI;
using OpenAI.Embeddings;

namespace TFS.Services;

public class Calculate
{
    public async Task Test()
    {
        //select from db to get alternatives
        


        const string options = "Host=localhost;Database=postgres;Username=postgres;Password=admin;";
        
        var dsb = new NpgsqlDataSourceBuilder(options);
        dsb.UseVector();
        await using var dataSource = dsb.Build();
        
        await using var conn = new NpgsqlConnection(options);
        try
        {
            conn.Open();

            var names = new List<string>();
            var sql = @"SELECT ""NAME"" FROM ""ROOTMAINLISTTFSTFEPARAMSParamAlt""";

            await using var cmd1 = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd1.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                names.Add(reader.GetString(0));
            }
            
            var items = names.Select(line =>
            {
                var lb = line.IndexOf('[');
                var rb = line.IndexOf(']');
                if (lb < 0 || rb <= lb) throw new Exception($"Bad line: {line}");

                var group = line.Substring(lb + 1, rb - lb - 1).Trim();
                var role = line.Substring(rb + 1).Trim();
                if (role.Length == 0) role = null;

                // Что эмбеддим. Обычно достаточно "group + (role?)"
                var full = role is null ? group : $"{group}. Роль: {role}";

                return new { Group = group, Role = role, Full = full };
            }).ToList();

        // 2) Считаем эмбеддинги
        var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var embedClient = new EmbeddingClient("text-embedding-3-small", apiKey);

        // Можно по одному (просто и надёжно)
        var vectors = new List<float[]>();
        foreach (var it in items)
        {
            try
            {
                var resp = embedClient.GenerateEmbeddingAsync(it.Full).Result;
                vectors.Add(resp.Value.ToFloats().ToArray());

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        // Подготовим команду «как в вашем примере»
        const string sql1 = @"
            INSERT INTO domain_objects(id, grp, role, full_text, embedding)
            VALUES (@id, @grp, @role, @full, @emb)";

        await using var conn2 = new NpgsqlConnection(options);
        conn2.Open();
        await using var tx = await conn2.BeginTransactionAsync();
        await using (var cmd = new NpgsqlCommand(sql1, conn2, tx))
        {
            // Чтобы не создавать параметры заново в цикле, объявим один раз:
            var pId   = cmd.Parameters.Add("id",   NpgsqlTypes.NpgsqlDbType.Uuid);
            var pGrp  = cmd.Parameters.Add("grp",  NpgsqlTypes.NpgsqlDbType.Text);
            var pRole = cmd.Parameters.Add("role", NpgsqlTypes.NpgsqlDbType.Text);
            var pFull = cmd.Parameters.Add("full", NpgsqlTypes.NpgsqlDbType.Text);

            
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                var vec = new Vector(vectors[i]);

                pId.Value   = Guid.NewGuid();
                pGrp.Value  = it.Group;
                pRole.Value = (object?)it.Role ?? DBNull.Value;
                pFull.Value = it.Full;
                
                cmd.Parameters.AddWithValue("emb", vec);


                await cmd.ExecuteNonQueryAsync();
            }
        }
        await tx.CommitAsync();

        Console.WriteLine("OK: вставлено " + items.Count + " записей.");
    

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}