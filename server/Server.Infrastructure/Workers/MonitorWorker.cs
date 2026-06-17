using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Service.Interfaces;

namespace Server.Infrastructure.Workers;

public class MonitorWorker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<MonitorWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Monitor Worker started at {Time}", DateTime.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();

                var monitorService =
                    scope.ServiceProvider.GetRequiredService<IMonitorService>();

                logger.LogInformation("Checking pending monitors...");

                await monitorService.ProcessPendingMonitors(stoppingToken);

                logger.LogInformation("Completed monitor check cycle.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while processing monitors.");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }

        logger.LogInformation("Monitor Worker stopped.");
    }
}