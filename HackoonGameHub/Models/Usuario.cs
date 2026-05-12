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
    
    public DateTime createdAt{get;set;}

    public Usuario(string username, string name, string email)
    {
        this.username = username;
        this.name = name;
        this.email = email;
        createdAt = DateTime.Now;
    }
    public Usuario(int id, string username, string name, string email, DateTime createdAt)
    {
        this.id = id;
        this.username = username;
        this.name = name;
        this.email = email;
        this.createdAt = createdAt;
    }
}