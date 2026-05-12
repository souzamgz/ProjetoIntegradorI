namespace Models;

public class Sala
{
    public int id { get; set; }
    public string name { get; set; }
    public string descricao { get; set; }

    public Sala(string name)
    {
        this.name = name;
    }

    public Sala(string name, string descricao)
    {
        this.name = name;
        this.descricao = descricao;
    }

    public Sala(int id, string name)
    {
        this.id = id;
        this.name = name;
    }
}