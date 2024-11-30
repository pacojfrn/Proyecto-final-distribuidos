namespace Rest.Dtos;

public class UserResponse{
    public int Id { get; set; }

    public required string? Name { get; set; } = null;

    public required string Persona { get; set; } = null;
}