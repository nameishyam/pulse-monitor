using Server.Domain.Enums;

namespace Server.Domain.Entities;

public class Monitor : BaseEntity
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int IntervalSeconds { get; set; }
    public string? RequestBody { get; set; }
    public HttpMethods HttpMethod { get; set; } = HttpMethods.Get;

    public int? HttpStatusCode { get; set; }
    public MonitorStatus MonitorStatus { get; set; } = MonitorStatus.Pending;
    public DateTime? LastChecked { get; set; }

    public User? User { get; init; }
    public ICollection<Log> Logs { get; init; } = new List<Log>();
}