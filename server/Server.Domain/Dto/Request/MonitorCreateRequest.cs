using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.Request;

public class MonitorCreateRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Url { get; set; } = string.Empty;
    [Required] public int IntervalSeconds { get; set; }
    [Required] public string HttpMethod { get; set; } = string.Empty;
    public string? RequestBody { get; set; }
}