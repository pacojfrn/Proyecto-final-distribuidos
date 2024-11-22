namespace Rest.Dtos;

public class CreateUserRequest{
    public required string Name { get; set; }
    public required List<string>? Persona { get; set; }
}