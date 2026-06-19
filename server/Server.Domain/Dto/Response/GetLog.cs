namespace Server.Domain.Dto.Response;

public class GetLog
{
    public int? ResponseTime { get; set; }
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}