using Server.Domain.Entities;

namespace Server.Service.Interfaces;

public interface ILogService
{
    Task Create(Log log);
    Task<ICollection<Log>> GetByMonitor(Guid monitorId);
}