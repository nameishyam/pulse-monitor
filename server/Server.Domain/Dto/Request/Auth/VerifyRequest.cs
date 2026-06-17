using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.Request.Auth
{
    public class VerifyRequest
    {
        [Required] public string Email { get; set; } = string.Empty;
        [Required] public int Otp { get; set; }
    }
}
