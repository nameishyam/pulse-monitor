using Microsoft.EntityFrameworkCore;
using Server.Domain.Dto.Request.Create;
using Server.Domain.Entities;
using Server.Domain.Interfaces.Repository;
using Server.Repository.Context;

namespace Server.Repository.Repositories;

public class LogRepository(ServerDbContext context) : ILogRepository
{
    public async Task Create(LogCreate request)
    {
        await context.Logs.AddAsync(new Log
        {
            MonitorId = request.MonitorId,
            ErrorMessage = request.ErrorMessage,
            ResponseTime = request.ResponseTime,
            StatusCode = request.StatusCode
        });

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