using Server.Domain.Dto.Request.Create;
using Server.Domain.Entities;

namespace Server.Domain.Interfaces.Repository;

public interface ILogRepository
{
    Task Create(LogCreate request);
    Task<ICollection<Log>> GetByMonitor(Guid monitorId);
}