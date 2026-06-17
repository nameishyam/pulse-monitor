using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Service.Interfaces;

public interface IMonitorService
{
    Task<ICollection<Monitor>> GetAll(Guid userId);
    Task<Guid> Create(MonitorCreateRequest request, Guid userId);
    Task Update(MonitorUpdateRequest request, Guid id);
    Task ProcessPendingMonitors(CancellationToken cancellationToken);
}