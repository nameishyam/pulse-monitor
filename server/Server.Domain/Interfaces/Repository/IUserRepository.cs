using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Entities;

namespace Server.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<bool> ExistsById(Guid userId);
    Task<string> UploadProfile(UploadProfile request);
    Task<User> Update(UpdateUser request);
}