using Server.Domain.Enums;

namespace Server.Domain.Entities;

public class Monitor : BaseEntity
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int IntervalSeconds { get; set; } = 60;
    public MonitorStatus MonitorStatus { get; set; } = MonitorStatus.Pending;
    public DateTime? LastChecked { get; set; }

    public User? User { get; set; }
    public ICollection<Log> Logs { get; set; } = new List<Log>();
}