namespace Server.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ProfileUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public int? Otp { get; set; }
    public DateTime? OtpExpiry { get; set; }

    public ICollection<Monitor> Monitors { get; set; } = new List<Monitor>();
}