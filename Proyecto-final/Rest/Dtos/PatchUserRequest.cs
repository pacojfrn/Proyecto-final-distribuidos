namespace Rest.Dtos;

public class PatchUserRequest{
    public string? Name { get; set; }
    public List<string>? Persona { get; set; }
}