using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface IMonitorService
{
    Task<GetMonitor> GetById(Guid monitorId);
    Task<GetMonitor> Create(MonitorCreate request, Guid userId);
    Task Update(MonitorUpdate request, Guid id);
    Task ProcessPendingMonitors(CancellationToken cancellationToken);
}