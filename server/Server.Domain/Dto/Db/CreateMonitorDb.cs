using Server.Domain.Enums;

namespace Server.Domain.Dto.Db;

public class CreateMonitorDb
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int IntervalSeconds { get; set; }
    public HttpMethods HttpMethod { get; set; } = HttpMethods.Get;
    public string? RequestBody { get; set; }
}