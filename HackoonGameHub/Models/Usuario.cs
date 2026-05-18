using System.Numerics;

namespace Models;

public class Usuario
{
    public int id { get; set; }
    
    public string username{get;set;}
    public string name{get;set;}
    public string email{get;set;}
    
    public int? id_sala {get;set;}
    public string password{get;set;}
    
    public string turma { get; set; }

    public int points { get; set; } = 0;
   
    public int level { get; set; } = 1;
    public int xp { get; set; } = 0;
    
    public int stars { get; set; } = 0;
    public DateTime createdAt{get;set;}

    public Usuario(string username, string name)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new Exception("Nick obrigatório");
        }

        if (username.Length < 3) 
        {
            throw new Exception("Nick muito curto");    /*Validação básica de Tamanho e filtro de caractéres especiais dos Nicks
                                                          para ajudar na interface e tirar caractéres impróprios */
        }

        if (username.Length > 12)
        {
            throw new Exception("Nick muito longo");
        }

        if (!username.All(c => char.IsLetterOrDigit(c) || c == ' '))
        {
            throw new Exception("Nick inválido");
        }
        
        this.username = this.username =
            char.ToUpper(username.Trim()[0]) +   //Deixar Nicks padronizados e bonitinhos (publico alvo mais jovem)
            username.Trim().Substring(1).ToLower();
        this.name = name.Trim();
        createdAt = DateTime.Now;
    }
    public Usuario(int id, string username, string name, DateTime createdAt)
    {
        this.id = id;
        this.username = username;
        this.name = name;
        this.createdAt = createdAt;
    }
    
    private int GetXpToNextLevel()
    {
        return 100 + (level * 25);
    }
    public void AddXp(int amount)
    {
        if (level >= 99)
        {
            level = 99;
            xp = 0;
            return;
        }

        xp += amount;

        while (xp >= GetXpToNextLevel())   //Balanceando o escalonamento do XP
        {
            xp -= GetXpToNextLevel();
            level++;

            if (level >= 99)
            {
                level = 99;
                xp = 0;
                break;
            }
        }
    }
}