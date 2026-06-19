namespace Server.Domain.Dto.Response;

public class GetMeResponse
{
    public UserResponse User { get; init; } = null!;
    public ICollection<MonitorResponse> Monitors { get; init; }
        = new List<MonitorResponse>();
}