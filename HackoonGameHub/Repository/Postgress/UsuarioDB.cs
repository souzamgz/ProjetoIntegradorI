using Models;

namespace Repository.Postgress;

// CRUD DE USUARIO
public class UsuarioDB
{
    public static async Task Create(Usuario usuario)
    {
        await using var cmd = DB.dataSource.CreateCommand(
            @"INSERT INTO usuarios (username, name, email, id_sala, password, created_at) values ($1, $2, $3, $4, $5, $6) returning id;");
        cmd.Parameters.AddWithValue(usuario.username);
        cmd.Parameters.AddWithValue(usuario.name);
        cmd.Parameters.AddWithValue(usuario.email);
        cmd.Parameters.AddWithValue(usuario.id_sala ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue(usuario.password ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue(usuario.createdAt);
        
        try
        {
            var res = await cmd.ExecuteScalarAsync();
            if (res != null)
            {
                usuario.id = Convert.ToInt32(res);
                Console.WriteLine("Usuario Criado com Sucesso!");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro ao criar Usuario: " + e.Message);
            throw;
        }

    }
    public static async Task<List<Usuario>> Listar()
    {
        var lista = new List<Usuario>();
        await using var cmd = DB.dataSource.CreateCommand("Select id, username, name, email, created_at from usuarios");
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var u = new Usuario(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetDateTime(4)
            );
            lista.Add(u);
        }
        return lista;
    }
    public static async Task<List<Usuario>> ListarPorSala(int id_sala)
    {
        var lista = new List<Usuario>();
        await using var cmd = DB.dataSource.CreateCommand("Select id, username, name, email, created_at, id_sala from usuarios where id_sala = $1");
        cmd.Parameters.AddWithValue(id_sala);
        await using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var u = new Usuario(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetDateTime(4)
            );
            u.id_sala = reader.IsDBNull(5) ? null : reader.GetInt32(5);
            lista.Add(u);
        }
        return lista;
    }
}