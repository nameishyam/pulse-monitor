using Microsoft.EntityFrameworkCore;
using Server.Domain.Dto.Db;
using Server.Domain.Interfaces.Repository;
using Server.Repository.Context;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Repository.Repositories;

public class MonitorRepository(ServerDbContext context) : IMonitorRepository
{
    public async Task<ICollection<Monitor>> GetAll(Guid userId)
    {
        return await context.Monitors
            .Where(m => m.UserId == userId)
            .ToListAsync();
    }

    public async Task<Monitor> GetById(Guid id)
    {
        return await context.Monitors.FirstAsync(m => m.Id == id);
    }

    public async Task<Guid> Create(CreateMonitorDb request)
    {
        var monitor = new Monitor
        {
            UserId = request.UserId,
            IntervalSeconds = request.IntervalSeconds,
            Name = request.Name,
            Url = request.Url
        };

        await context.Monitors.AddAsync(monitor);
        await context.SaveChangesAsync();

        return monitor.Id;
    }

    public async Task Update(UpdateMonitorDb request)
    {
        var monitor = await context.Monitors
            .FirstAsync(x => x.Id == request.Id);

        monitor.IntervalSeconds = request.IntervalSeconds ?? monitor.IntervalSeconds;
        monitor.MonitorStatus = request.MonitorStatus ?? monitor.MonitorStatus;
        monitor.LastChecked = request.LastChecked ?? monitor.LastChecked;
        monitor.NextChecked = request.NextChecked ?? monitor.NextChecked;

        await context.SaveChangesAsync();
    }

    public async Task<ICollection<Monitor>> GetPendingMonitors()
    {
        return await context.Monitors
            .Where(x => x.NextChecked <= DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<bool> ExistsById(Guid monitorId)
    {
        return await context.Monitors.AnyAsync(m => m.Id == monitorId);
    }
}