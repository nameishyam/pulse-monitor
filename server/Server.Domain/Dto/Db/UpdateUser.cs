namespace Server.Domain.Dto.Db;

public class UpdateUser
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfileUrl { get; set; }
    public string? Bio { get; set; }
}