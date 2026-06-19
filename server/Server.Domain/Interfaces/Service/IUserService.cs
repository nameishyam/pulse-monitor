using Microsoft.AspNetCore.Http;
using Server.Domain.Dto.Request.Update;

namespace Server.Domain.Interfaces.Service;

public interface IUserService
{
    Task<string> Upload(IFormFile fileData, Guid userId);
    Task Update(UserUpdateRequest request, Guid userId);
}