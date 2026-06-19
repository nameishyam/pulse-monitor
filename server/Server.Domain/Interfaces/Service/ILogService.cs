using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface ILogService
{
    Task<ICollection<GetLog>> GetByMonitor(Guid monitorId);
}