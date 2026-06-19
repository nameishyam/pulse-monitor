namespace Server.Domain.Dto.Response;

public class GetMe
{
    public GetUser User { get; init; } = null!;
    public ICollection<GetMonitor> Monitors { get; init; }
        = new List<GetMonitor>();
}