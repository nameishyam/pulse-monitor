using Server.Domain.Enums;

namespace Server.Domain.Dto.Db;

public class UpdateMonitorDb
{
    public Guid Id { get; set; }
    public int? IntervalSeconds { get; set; }
    public string? RequestBody { get; set; }
    public HttpMethods? HttpMethod { get; set; }
    public MonitorStatus? MonitorStatus { get; set; }
    public DateTime? NextChecked { get; set; }
    public DateTime? LastChecked { get; set; }
}