namespace Server.Domain.Entities;

public class Log : BaseEntity
{
    public Guid MonitorId { get; set; }
    public int ResponseTime { get; set; }
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }

    public Monitor Monitor { get; set; } = null!;
}