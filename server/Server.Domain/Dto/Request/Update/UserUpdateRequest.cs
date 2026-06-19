namespace Server.Domain.Dto.Request.Update;

public class UserUpdateRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? ProfileUrl { get; set; }
    public string? Bio { get; set; }
}