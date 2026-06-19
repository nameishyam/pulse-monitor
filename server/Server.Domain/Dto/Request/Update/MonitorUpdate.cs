using Server.Domain.Enums;

namespace Server.Domain.Dto.Request.Update;

public class MonitorUpdate
{
    public int? IntervalSeconds { get; set; }
    public MonitorStatus? MonitorStatus { get; set; }
    public DateTime? LastChecked { get; set; }
}