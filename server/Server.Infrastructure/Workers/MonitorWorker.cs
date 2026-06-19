using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.Domain.Interfaces.Service;

namespace Server.Infrastructure.Workers;

public class MonitorWorker(
    IServiceScopeFactory serviceScopeFactory,
    ILogger<MonitorWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Monitor Worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();

                var monitorService =
                    scope.ServiceProvider.GetRequiredService<IMonitorService>();

                await monitorService.ProcessPendingMonitors(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Background worker failed.");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}