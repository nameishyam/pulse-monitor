using Server.Domain.Dto.Response;
using System.Diagnostics;
using Server.Domain.Interfaces.Infrastructure;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Infrastructure.Services;

public class MonitorChecker(IHttpClientFactory httpClientFactory)
    : IMonitorChecker
{
    public async Task<MonitorCheck> CheckAsync(
        Monitor monitor,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        try
        {
            using var request = new HttpRequestMessage(
                new HttpMethod("GET"),
                monitor.Url);

            var stopwatch = Stopwatch.StartNew();

            var response = await client.SendAsync(request, cancellationToken);

            stopwatch.Stop();

            return new MonitorCheck
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                ResponseTime = (int)stopwatch.ElapsedMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new MonitorCheck
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}