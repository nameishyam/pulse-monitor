namespace Server.Domain.Dto.Response;

public class MonitorCheck
{
    public bool IsSuccess { get; set; }
    public int? StatusCode { get; set; }
    public int? ResponseTime { get; set; }
    public string? ErrorMessage { get; set; }
}