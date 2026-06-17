using Microsoft.Extensions.Logging;
using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Entities;
using Server.Domain.Enums;
using Server.Repository.Interfaces;
using Server.Service.Exceptions;
using Server.Service.Interfaces;
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

    public async Task<Guid> Create(MonitorCreateRequest request, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new InvalidDetailsException("name of the monitor is not valid");
        }

        if (string.IsNullOrWhiteSpace(request.Url))
        {
            throw new InvalidDetailsException("url is invalid");
        }

        if (!Enum.TryParse<HttpMethods>(
                request.HttpMethod,
                ignoreCase: true,
                out var httpMethod))
        {
            throw new InvalidDetailsException("Invalid HTTP method.");
        }

        return await monitorRepository.Create(new CreateMonitorDb
        {
            UserId = userId,
            IntervalSeconds = request.IntervalSeconds,
            Name = request.Name,
            Url = request.Url,
            RequestBody = request.RequestBody,
            HttpMethod = httpMethod
        });
    }

    public async Task Update(MonitorUpdateRequest request, Guid id)
    {
        if (await monitorRepository.GetById(id) == null)
        { 
            throw new NotFoundException($"monitor with the id {id} not found");
        }

        if (!Enum.TryParse<HttpMethods>(
                request.HttpMethod,
                ignoreCase: true,
                out var httpMethod))
        {
            throw new InvalidDetailsException("Invalid HTTP method.");
        }

        await monitorRepository.Update(new UpdateMonitorDb
        {
            Id = id,
            HttpMethod = httpMethod,
            HttpStatusCode = request.HttpStatusCode,
            IntervalSeconds = request.IntervalSeconds,
            LastChecked = request.LastChecked,
            MonitorStatus = request.MonitorStatus,
            RequestBody = request.RequestBody
        });
    }

    public async Task ProcessPendingMonitors(CancellationToken cancellationToken)
    {
        var monitors = await monitorRepository.GetPendingMonitors();

        logger.LogInformation("Found {Count} monitor(s) to process.", monitors.Count);

        foreach (var monitor in monitors)
        {
            logger.LogInformation(
                "Checking Monitor: {MonitorName} ({MonitorId}) - {Url}",
                monitor.Name,
                monitor.Id,
                monitor.Url);

            var result = await monitorChecker.CheckAsync(
                monitor,
                cancellationToken);

            logger.LogInformation(
                "Status: {Status}, StatusCode: {StatusCode}, ResponseTime: {ResponseTime} ms",
                result.IsSuccess ? "UP" : "DOWN",
                result.StatusCode,
                result.ResponseTime);

            await monitorRepository.Update(new UpdateMonitorDb
            {
                Id = monitor.Id,
                HttpStatusCode = result.StatusCode,
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
                "Database updated and log inserted for monitor {MonitorId}",
                monitor.Id);
        }
    }
}