using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Response;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;

namespace Server.Service.Services;

public class LogService(ILogRepository logRepository) : ILogService
{
    public async Task Create(LogCreate request)
    {
        await logRepository.Create(request);
    }

    public async Task<ICollection<GetLog>> GetByMonitor(Guid monitorId)
    {
        return (await logRepository.GetByMonitor(monitorId)).Select(l => new GetLog
        {
            ResponseTime = l.ResponseTime,
            ErrorMessage = l.ErrorMessage,
            StatusCode = l.StatusCode,
            CreatedAt = l.CreatedAt
        })
        .ToList();
    }
}