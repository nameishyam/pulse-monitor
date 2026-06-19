using Server.Domain.Entities;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;

namespace Server.Service.Services;

public class LogService(ILogRepository logRepository) : ILogService
{
    public async Task Create(Log log)
    {
        await logRepository.Create(log);
    }

    public async Task<ICollection<Log>> GetByMonitor(Guid monitorId)
    {
        return await logRepository.GetByMonitor(monitorId);
    }
}