using Server.Domain.Entities;

namespace Server.Domain.Interfaces.Service;

public interface ILogService
{
    Task Create(Log log);
    Task<ICollection<Log>> GetByMonitor(Guid monitorId);
}