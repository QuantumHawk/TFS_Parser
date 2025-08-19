using Npgsql;
using Pgvector;
namespace TFS.Services;

public class Calculate
{
    public async Task Test()
    {
        //select from db to get alternatives
        
        await using var conn = new NpgsqlConnection("Host=localhost;Database=postgres;Username=postgres;Password=admin;");
        await conn.OpenAsync();

        var embeddingFloatArray = "";

        var insert = new NpgsqlCommand("INSERT INTO classes(id,name,description,embedding) VALUES (@id,@n,@d,@e)", conn);
        insert.Parameters.AddWithValue("id","Order");
        insert.Parameters.AddWithValue("n","Order");
        insert.Parameters.AddWithValue("d","Заказ с позициями, статусом и суммой");
        insert.Parameters.AddWithValue("e", new Vector(embeddingFloatArray));
        await insert.ExecuteNonQueryAsync();

        var search = new NpgsqlCommand(@"
          SELECT id,name,description
          FROM classes
          ORDER BY embedding <-> @q
          LIMIT 5", conn);

        var queryEmbedding = "";
        search.Parameters.AddWithValue("q", new Vector(queryEmbedding));
    }
}