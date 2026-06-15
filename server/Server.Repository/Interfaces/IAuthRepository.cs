using Server.Domain.Entities;

namespace Server.Repository.Interfaces;

public interface IAuthRepository
{
    Task Create(User user);
    Task Update(User user);
    Task ClearOtp(Guid userId);
    Task<bool> ExistsByEmail(string email);
    Task<User> GetByEmail(string email);
    Task<User?> GetById(Guid id);
}