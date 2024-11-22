namespace Rest.Models;

public class UserModel{

    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<string> Persona { get; set; }
}