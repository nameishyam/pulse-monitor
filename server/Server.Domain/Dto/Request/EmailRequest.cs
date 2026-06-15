using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.Request;

public class EmailRequest
{
    [Required] public string Email { get; set; } = string.Empty;
}