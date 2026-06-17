using Server.Domain.Entities;

namespace Server.Repository.Interfaces;

public interface ILogRepository
{
    Task Create(Log log);
    Task<ICollection<Log>> GetByMonitor(Guid monitorId);
}