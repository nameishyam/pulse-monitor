using Server.Domain.Dto.Response;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Service.Interfaces;

public interface IMonitorChecker
{
    Task<MonitorCheckResult> CheckAsync(Monitor monitor, CancellationToken cancellationToken);
}