using Server.Domain.Enums;

namespace Server.Domain.Dto.Response;

public class MonitorResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
    public int IntervalSeconds { get; set; }
    public string? RequestBody { get; set; }
    public HttpMethods HttpMethod { get; set; } = HttpMethods.Get;

    public MonitorStatus MonitorStatus { get; set; } = MonitorStatus.Pending;
    public DateTime? LastChecked { get; set; }
}