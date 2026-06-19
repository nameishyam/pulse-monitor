using System.ComponentModel.DataAnnotations;

namespace Server.Domain.Dto.Request.Create;

public class MonitorCreate
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Url { get; set; } = string.Empty;
    [Required] public int IntervalSeconds { get; set; }
}