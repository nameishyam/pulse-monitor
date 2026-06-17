namespace Server.Domain.Dto.Response;

public class MonitorCheckResult
{
    public bool IsSuccess { get; set; }
    public int? StatusCode { get; set; }
    public int? ResponseTime { get; set; }
    public string? ErrorMessage { get; set; }
}