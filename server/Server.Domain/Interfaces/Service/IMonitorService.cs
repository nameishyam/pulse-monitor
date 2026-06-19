using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Domain.Interfaces.Service;

public interface IMonitorService
{
    Task<ICollection<Monitor>> GetAll(Guid userId);
    Task<MonitorResponse> Create(MonitorCreateRequest request, Guid userId);
    Task Update(MonitorUpdateRequest request, Guid id);
    Task ProcessPendingMonitors(CancellationToken cancellationToken);
}