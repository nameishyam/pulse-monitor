using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request.Update;

namespace Server.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<bool> ExistsById(Guid userId);
    Task<string> UploadProfile(UploadProfile request);
    Task Update(UpdateUser request);
}