using System.Diagnostics;
using Npgsql;
using Repository.Exceptions;

namespace Repository.Postgress;

public class DB
{
    public static NpgsqlDataSource dataSource {get; private set;}
    public static bool conectado = false;
    
    //Funcão deve rodar apenas uma vez ao inciar o game e TEM QUE DAR CERTO
    public static async Task connect()
    {
        var connectionString = "Host=127.0.0.1;Port=5432;Username=postgres;Database=postgres;Password="; // login do banco e local doo banco na rede
        dataSource = NpgsqlDataSource.Create(connectionString); // Inicializa a conexão
        
        while (!conectado)
        {
            try
            {
                //tenta conectar
                await using var connection = await dataSource.OpenConnectionAsync();
                Console.WriteLine("Conectado: " + connection);
                conectado = true;
            }
            catch (Exception e)
            {
                //se der erro vai tentando a cada 1 Segundo
                Console.WriteLine("Erro ao conectar: " + e);
                await Task.Delay(1000);
            }
        }
    }
    public  static void testConnection()
    {
        if (!conectado || dataSource == null)
        {
            throw new DatabaseNotConnectedException();
        }   
    }
    public static async Task Setup()
    {
        await using var cmd = dataSource.CreateCommand(@"
        

        -- Tabela de Salas
        CREATE TABLE IF NOT EXISTS salas (
            id SERIAL primary key,
	        name varchar(255) not null,
	        descricao text,
	        created_at timestamp
        );

        -- Tabela de Usuários (com FK para salas)
        CREATE TABLE IF NOT EXISTS usuarios (
        	id SERIAL primary KEY,
	        username varchar(255) unique not null,
	        name varchar(255) not null,
	        email varchar(255) unique,
	        id_sala INTEGER references salas(id) on delete set null,
	        password varchar(255),
	        created_at timestamp
        );
    ");

        await cmd.ExecuteNonQueryAsync();
        Console.WriteLine("[DB] Tabelas verificadas/criadas com sucesso.");
    }
    public static void StartDatabase()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        // No Windows, o binário terá a extensão .exe
        string pgctlName = OperatingSystem.IsWindows() ? "pg_ctl.exe" : "pg_ctl";
        string pgctlPath = Path.Combine(baseDir, "bin", pgctlName);
        string dataPath = Path.Combine(baseDir, "data");
        string logPath = Path.Combine(baseDir, "logfile");

        // Preparação específica para Linux (Debian)
        if (OperatingSystem.IsLinux())
        {
            // O Windows não exige chmod 700, mas o Linux sim
            Process.Start("chmod", $"700 \"{dataPath}\"").WaitForExit();
        
            // Garante as pastas técnicas que o Linux exige
            string[] folders = { "pg_tblspc", "pg_replslot", "pg_snapshots", "pg_commit_ts" };
            foreach (var folder in folders)
            {
                string path = Path.Combine(dataPath, folder);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
        }

        // Limpeza de arquivos de trava (Funciona em ambos)
        string pidFile = Path.Combine(dataPath, "postmaster.pid");
        if (File.Exists(pidFile)) File.Delete(pidFile);

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = pgctlPath,
            Arguments = $"-D \"{dataPath}\" -l \"{logPath}\" start",
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = baseDir 
        };

        Process.Start(psi);
    }
}