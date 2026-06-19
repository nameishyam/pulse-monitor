using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Entities;
using Server.Domain.Interfaces.Repository;
using Server.Repository.Context;
using Supabase;
using SupabaseOptions = Server.Domain.Dto.Options.SupabaseOptions;

namespace Server.Repository.Repositories;

public class UserRepository(
    Client supabase,
    IOptions<SupabaseOptions> options,
    ServerDbContext context) : IUserRepository
{
    private readonly SupabaseOptions _supabaseOptions = options.Value;

    public async Task<bool> ExistsById(Guid userId)
    {
        return await context.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<string> UploadProfile(UploadProfile request)
    {
        await supabase.Storage
            .From(_supabaseOptions.BucketName)
            .Upload(
                request.Bytes,
                request.FileName,
                new Supabase.Storage.FileOptions
                {
                    ContentType = request.ContentType,
                    Upsert = true
                });

        return supabase.Storage
            .From(_supabaseOptions.BucketName)
            .GetPublicUrl(request.FileName);
    }

    public async Task<User> Update(UpdateUser request)
    {
        var existingUser = await context.Users.FirstAsync(u => u.Id == request.Id);

        existingUser.FirstName = request.FirstName ?? existingUser.FirstName;
        existingUser.LastName = request.LastName ?? existingUser.LastName;
        existingUser.Bio = request.Bio ?? existingUser.Bio;
        existingUser.Email = request.Email ?? existingUser.Email;
        existingUser.ProfileUrl = request.ProfileUrl ?? existingUser.ProfileUrl;

        await context.SaveChangesAsync();
        return existingUser;
    }
}