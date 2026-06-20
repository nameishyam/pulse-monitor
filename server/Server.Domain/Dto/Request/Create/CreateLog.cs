namespace Server.Domain.Dto.Request.Create;

public class LogCreate
{
    public Guid MonitorId { get; set; }
    public int? ResponseTime { get; set; }
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}