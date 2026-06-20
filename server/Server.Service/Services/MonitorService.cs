using Microsoft.Extensions.Logging;
using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;
using Server.Domain.Enums;
using Server.Domain.Interfaces.Infrastructure;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;

namespace Server.Service.Services;

public class MonitorService(
    IMonitorRepository monitorRepository,
    ILogService logService,
    IMonitorChecker monitorChecker,
    ILogger<MonitorService> logger) : IMonitorService
{
    public async Task<GetMonitor> GetById(Guid monitorId)
    {
        if (!await monitorRepository.ExistsById(monitorId))
        {
            throw new NotFoundException("monitor with the given id not found");
        }

        var monitor = await monitorRepository.GetById(monitorId);

        return new GetMonitor
        {
            Id = monitorId,
            Name = monitor.Name,
            Url = monitor.Url,
            IntervalSeconds = monitor.IntervalSeconds,
            LastChecked = monitor.LastChecked,
            MonitorStatus = monitor.MonitorStatus
        };
    }

    public async Task<GetMonitor> Create(MonitorCreate request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidDetailsException("name of the monitor is not valid");
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            throw new InvalidDetailsException("url is invalid");
        }

        var monitorId = await monitorRepository.Create(new CreateMonitor
        {
            UserId = userId,
            IntervalSeconds = request.IntervalSeconds,
            Name = request.Name,
            Url = request.Url
        });

        var monitor = await monitorRepository.GetById(monitorId);

        return new GetMonitor
        {
            Id = monitorId,
            IntervalSeconds = monitor.IntervalSeconds,
            Name = monitor.Name,
            MonitorStatus = monitor.MonitorStatus,
            Url = monitor.Url,
            LastChecked = monitor.LastChecked
        };
    }

    public async Task Update(MonitorUpdate request, Guid id)
    {
        if (!await monitorRepository.ExistsById(id))
        { 
            throw new NotFoundException($"monitor with the id {id} not found");
        }

        await monitorRepository.Update(new UpdateMonitor
        {
            Id = id,
            IntervalSeconds = request.IntervalSeconds,
            LastChecked = request.LastChecked,
            MonitorStatus = request.MonitorStatus
        });
    }

    public async Task ProcessPendingMonitors(CancellationToken cancellationToken)
    {
        var monitors = await monitorRepository.GetPendingMonitors();

        foreach (var monitor in monitors)
        {
            var result = await monitorChecker.CheckAsync(
                monitor,
                cancellationToken);

            await monitorRepository.Update(new UpdateMonitor
            {
                Id = monitor.Id,
                MonitorStatus = result.IsSuccess
                    ? MonitorStatus.Up
                    : MonitorStatus.Down,
                LastChecked = DateTime.UtcNow,
                NextChecked = DateTime.UtcNow.AddSeconds(monitor.IntervalSeconds)
            });

            await logService.Create(new LogCreate
            {
                MonitorId = monitor.Id,
                ResponseTime = result.ResponseTime,
                StatusCode = result.StatusCode,
                ErrorMessage = result.ErrorMessage
            });

            logger.LogInformation(
                "[{Timestamp}] {MonitorName} ({Url}) => {Status} ({StatusCode}) | {ResponseTime} ms",
                DateTime.UtcNow,
                monitor.Name,
                monitor.Url,
                result.IsSuccess ? "UP" : "DOWN",
                result.StatusCode,
                result.ResponseTime);
        }
    }
}