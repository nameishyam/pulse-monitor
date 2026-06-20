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
    IEmailService emailService,
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
            var previousStatus = monitor.MonitorStatus;

            var result = await monitorChecker.CheckAsync(
                monitor,
                cancellationToken);

            var currentStatus = result.IsSuccess
                ? MonitorStatus.Up
                : MonitorStatus.Down;

            await monitorRepository.Update(new UpdateMonitor
            {
                Id = monitor.Id,
                MonitorStatus = currentStatus,
                LastChecked = DateTime.UtcNow,
                NextChecked = DateTime.UtcNow.AddSeconds(monitor.IntervalSeconds)
            });

            if (previousStatus != MonitorStatus.Pending &&
                previousStatus != currentStatus)
            {
                if (currentStatus == MonitorStatus.Down)
                {
                    await emailService.SendEmailAsync(
                        monitor.User!.Email,
                        "🚨 Pulse Monitor Alert: Endpoint Down",
                        $"""
                    <html>
                    <body style="font-family: Arial, sans-serif; line-height:1.6;">
                        <h2 style="color:#d32f2f;">Endpoint Down</h2>

                        <p>Your monitored endpoint is currently unavailable.</p>

                        <table>
                            <tr><td><strong>Monitor</strong></td><td>{monitor.Name}</td></tr>
                            <tr><td><strong>URL</strong></td><td>{monitor.Url}</td></tr>
                            <tr><td><strong>Status Code</strong></td><td>{result.StatusCode?.ToString() ?? "No Response"}</td></tr>
                            <tr><td><strong>Response Time</strong></td><td>{result.ResponseTime?.ToString() ?? "-"} ms</td></tr>
                            <tr><td><strong>Error</strong></td><td>{result.ErrorMessage ?? "Unknown Error"}</td></tr>
                            <tr><td><strong>Detected At (UTC)</strong></td><td>{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}</td></tr>
                        </table>

                        <br/>

                        <p>Please investigate the endpoint to restore service.</p>

                        <hr/>

                        <p style="font-size:12px;color:gray;">
                            This notification was generated automatically by Pulse Monitor.
                        </p>
                    </body>
                    </html>
                    """);
                }
                else
                {
                    await emailService.SendEmailAsync(
                        monitor.User!.Email,
                        "✅ Pulse Monitor: Endpoint Recovered",
                        $"""
                    <html>
                    <body style="font-family: Arial, sans-serif; line-height:1.6;">
                        <h2 style="color:#2e7d32;">Endpoint Recovered</h2>

                        <p>Your monitored endpoint is responding normally again.</p>

                        <table>
                            <tr><td><strong>Monitor</strong></td><td>{monitor.Name}</td></tr>
                            <tr><td><strong>URL</strong></td><td>{monitor.Url}</td></tr>
                            <tr><td><strong>Status Code</strong></td><td>{result.StatusCode}</td></tr>
                            <tr><td><strong>Response Time</strong></td><td>{result.ResponseTime} ms</td></tr>
                            <tr><td><strong>Recovered At (UTC)</strong></td><td>{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}</td></tr>
                        </table>

                        <br/>

                        <p>No further action is required unless the issue reoccurs.</p>

                        <hr/>

                        <p style="font-size:12px;color:gray;">
                            This notification was generated automatically by Pulse Monitor.
                        </p>
                    </body>
                    </html>
                    """);
                }
            }

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
                currentStatus,
                result.StatusCode,
                result.ResponseTime);
        }
    }
}