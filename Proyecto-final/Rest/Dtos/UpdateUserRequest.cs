namespace Rest.Dtos
{
    public class UpdateUserRequest
    {
        public required string? Name { get; set; }
        public required List<string>? Persona { get; set; }
    }
}
