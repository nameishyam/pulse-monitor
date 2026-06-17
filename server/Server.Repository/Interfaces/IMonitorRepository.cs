using Server.Domain.Dto.Db;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Repository.Interfaces;

public interface IMonitorRepository
{
    Task<ICollection<Monitor>> GetAll(Guid userId);
    Task<Monitor?> GetById(Guid id);
    Task<Guid> Create(CreateMonitorDb request);
    Task Update(UpdateMonitorDb request);
    Task<ICollection<Monitor>> GetPendingMonitors();
}