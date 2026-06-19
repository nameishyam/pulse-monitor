using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;
using Server.Domain.Interfaces.Repository;
using Server.Repository.Context;

namespace Server.Repository.Repositories;

public class LogRepository(ServerDbContext context) : ILogRepository
{
    public async Task Create(Log log)
    {
        await context.Logs.AddAsync(log);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Log>> GetByMonitor(Guid monitorId)
    {
        return await context.Logs
            .Where(x => x.MonitorId == monitorId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}