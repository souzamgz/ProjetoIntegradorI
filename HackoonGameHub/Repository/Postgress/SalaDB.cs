using Models;

namespace Repository.Postgress;
//CRUD DE SALA
public class SalaDB
{
    public static async Task Create(Sala sala)
    {
        await using var cmd = DB.dataSource.CreateCommand(
            @"INSERT INTO salas (name, descricao) values ($1, $2) returning id;");
        cmd.Parameters.AddWithValue(sala.name);
        cmd.Parameters.AddWithValue(sala.descricao ?? (object)DBNull.Value);

        try
        {
            var res = await cmd.ExecuteScalarAsync();
            if (res != null)
            {
                sala.id = Convert.ToInt32(res);
                Console.WriteLine("Sala criada com sucesso!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao criar sala: " + e.Message);
            throw;
        }
        
    }
}