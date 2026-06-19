using Server.Domain.Entities;

namespace Server.Domain.Interfaces.Repository;

public interface ILogRepository
{
    Task Create(Log log);
    Task<ICollection<Log>> GetByMonitor(Guid monitorId);
}