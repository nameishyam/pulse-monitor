using Server.Domain.Enums;

namespace Server.Domain.Dto.Request.Update;

public class MonitorUpdateRequest
{
    public int? IntervalSeconds { get; set; }
    public string? RequestBody { get; set; }
    public string? HttpMethod { get; set; }
    public MonitorStatus? MonitorStatus { get; set; }
    public DateTime? LastChecked { get; set; }
}