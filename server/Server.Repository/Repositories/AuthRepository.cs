using Microsoft.EntityFrameworkCore;
using Server.Domain.Entities;
using Server.Repository.Context;
using Server.Repository.Interfaces;

namespace Server.Repository.Repositories;

public class AuthRepository(ServerDbContext context) : IAuthRepository
{
    public async Task Create(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        var existingUser = await context.Users
            .FirstAsync(u => u.Id == user.Id);

        existingUser.FirstName = user.FirstName ?? existingUser.FirstName;
        existingUser.LastName = user.LastName ?? existingUser.LastName;
        existingUser.Email = user.Email ?? existingUser.Email;
        existingUser.Password = user.Password ?? existingUser.Password;

        await context.SaveChangesAsync();
    }

    public async Task ClearOtp(Guid userId)
    {
        var user = await context.Users.FirstAsync(u => u.Id == userId);

        user.Otp = null;
        user.OtpExpiry = null;

        await context.SaveChangesAsync();
    }

    public async Task<User> GetByEmail(string email)
    {
        return await context.Users.FirstAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByEmail(string email)
    {
        return await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<User?> GetById(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}