using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Enums;
using Server.Repository.Interfaces;
using Server.Service.Exceptions;
using Server.Service.Interfaces;
using Monitor = Server.Domain.Entities.Monitor;

namespace Server.Service.Services;

public class MonitorService(IMonitorRepository monitorRepository) : IMonitorService
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
}