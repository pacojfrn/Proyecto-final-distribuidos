namespace Rest.Infraestructure.Entities;

public class UserEntity{

    public int Id { get; set; }

    public string? Name { get; set; }

    public List<string>? Persona { get; set; }
}