using Models;
using Repository.Postgress;

namespace Repository;

public class Rodando
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Classe para Testar Repository!");
        
       await DB.connect();

       Sala sala = new Sala("3°B");

       Usuario usuario = new Usuario("pedro", "pedro", "pedro@gmail.com");
       usuario.id_sala = 2;

       await UsuarioDB.Create(usuario);
       //await SalaDB.Create(sala);
       
        List<Usuario> users = await UsuarioDB.ListarPorSala(2);
        foreach (var user in users)
        {
         Console.WriteLine(user.id);
         Console.WriteLine(user.name);
         Console.WriteLine(user.username);
         Console.WriteLine(user.email);
         Console.WriteLine(user.createdAt);
         Console.WriteLine(string.IsNullOrEmpty(user.password) ? "sem senha" : user.password);
        }
    }
}