using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface ILogService
{
    Task<ICollection<GetLog>> GetByMonitor(Guid monitorId);
    Task Create(LogCreate request);
}