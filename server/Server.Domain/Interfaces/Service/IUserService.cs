using Microsoft.AspNetCore.Http;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface IUserService
{
    Task<string> Upload(IFormFile fileData, Guid userId);
    Task<UserResponse> Update(UserUpdateRequest request, Guid userId);
}