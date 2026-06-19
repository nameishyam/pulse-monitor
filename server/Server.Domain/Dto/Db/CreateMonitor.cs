namespace Server.Domain.Dto.Db;

public class CreateMonitor
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int IntervalSeconds { get; set; }
}