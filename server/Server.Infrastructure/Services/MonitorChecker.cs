using Server.Domain.Dto.Response;
using System.Diagnostics;
using System.Text;
using Server.Domain.Interfaces.Infrastructure;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Infrastructure.Services;

public class MonitorChecker(IHttpClientFactory httpClientFactory)
    : IMonitorChecker
{
    public async Task<MonitorCheckResult> CheckAsync(
        Monitor monitor,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient();

        try
        {
            using var request = new HttpRequestMessage(
                new HttpMethod(monitor.HttpMethod.ToString().ToUpper()),
                monitor.Url);

            if (!string.IsNullOrWhiteSpace(monitor.RequestBody))
            {
                request.Content = new StringContent(
                    monitor.RequestBody,
                    Encoding.UTF8,
                    "application/json");
            }

            var stopwatch = Stopwatch.StartNew();

            var response = await client.SendAsync(request, cancellationToken);

            stopwatch.Stop();

            return new MonitorCheckResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = (int)response.StatusCode,
                ResponseTime = (int)stopwatch.ElapsedMilliseconds
            };
        }
        catch (Exception ex)
        {
            return new MonitorCheckResult
            {
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
    }
}