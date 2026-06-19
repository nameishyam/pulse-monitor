using Server.Domain.Dto.Db;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Domain.Interfaces.Repository;

public interface IMonitorRepository
{
    Task<ICollection<Monitor>> GetAll(Guid userId);
    Task<Monitor> GetById(Guid id);
    Task<Guid> Create(CreateMonitorDb request);
    Task Update(UpdateMonitorDb request);
    Task<ICollection<Monitor>> GetPendingMonitors();
    Task<bool> ExistsById(Guid monitorId);
}