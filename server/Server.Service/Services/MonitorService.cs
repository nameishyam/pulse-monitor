using Microsoft.Extensions.Logging;
using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Domain.Interfaces.Infrastructure;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Service.Services;

public class MonitorService(
    IMonitorRepository monitorRepository,
    ILogRepository logRepository,
    IMonitorChecker monitorChecker,
    ILogger<MonitorService> logger) : IMonitorService
{
    public async Task<ICollection<Monitor>> GetAll(Guid userId)
    {
        return await monitorRepository.GetAll(userId);
    }

    public async Task<MonitorResponse> Create(MonitorCreateRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidDetailsException("name of the monitor is not valid");
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            throw new InvalidDetailsException("url is invalid");
        }

        var monitorId = await monitorRepository.Create(new CreateMonitorDb
        {
            UserId = userId,
            IntervalSeconds = request.IntervalSeconds,
            Name = request.Name,
            Url = request.Url
        });

        if (!await monitorRepository.ExistsById(monitorId))
        {
            throw new NotFoundException("monitor with the details not found");
        }

        var monitor = await monitorRepository.GetById(monitorId);

        return new MonitorResponse
        {
            Id = monitorId,
            IntervalSeconds = monitor.IntervalSeconds,
            Name = monitor.Name,
            MonitorStatus = monitor.MonitorStatus,
            Url = monitor.Url,
            LastChecked = monitor.LastChecked
        };
    }

    public async Task Update(MonitorUpdateRequest request, Guid id)
    {
        if (!await monitorRepository.ExistsById(id))
        { 
            throw new NotFoundException($"monitor with the id {id} not found");
        }

        await monitorRepository.Update(new UpdateMonitorDb
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

            await monitorRepository.Update(new UpdateMonitorDb
            {
                Id = monitor.Id,
                MonitorStatus = result.IsSuccess
                    ? MonitorStatus.Up
                    : MonitorStatus.Down,
                LastChecked = DateTime.UtcNow,
                NextChecked = DateTime.UtcNow.AddSeconds(monitor.IntervalSeconds)
            });

            await logRepository.Create(new Log
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