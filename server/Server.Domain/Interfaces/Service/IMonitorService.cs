using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Domain.Interfaces.Service;

public interface IMonitorService
{
    Task<ICollection<Monitor>> GetAll(Guid userId);
    Task<GetMonitor> Create(MonitorCreate request, Guid userId);
    Task Update(MonitorUpdate request, Guid id);
    Task ProcessPendingMonitors(CancellationToken cancellationToken);
}