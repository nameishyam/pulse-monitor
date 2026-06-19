using Microsoft.AspNetCore.Http;
using Server.Domain.Dto.Db;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Dto.Response;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;

namespace Server.Service.Services;

public class UserService(IUserRepository userRepository) : IUserService
{ 
    public async Task<string> Upload(IFormFile fileData, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(fileData.FileName))
        {
            throw new InvalidDetailsException("Invalid file name.");
        }

        if (!fileData.ContentType.StartsWith("image/"))
        {
            throw new InvalidDetailsException("Only images are allowed.");
        }

        await using var stream = fileData.OpenReadStream();

        using var memoryStream = new MemoryStream();

        await stream.CopyToAsync(memoryStream);

        var imageUrl = await userRepository.UploadProfile(new UploadProfile
        {
            Bytes = memoryStream.ToArray(),
            FileName = $"{userId}/{Guid.NewGuid()}{Path.GetExtension(fileData.FileName)}",
            ContentType = fileData.ContentType
        });

        await Update(new UserUpdateRequest
        {
            ProfileUrl = imageUrl
        }, userId);

        return imageUrl;
    }

    public async Task<UserResponse> Update(UserUpdateRequest request, Guid userId)
    {
        if (!await userRepository.ExistsById(userId))
        {
            throw new NotFoundException("user with the given id not found");
        }

        var updatedUser = await userRepository.Update(new UpdateUser
        {
            Id = userId,
            Bio = request.Bio,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            ProfileUrl = request.ProfileUrl
        });

        return new UserResponse
        {
            Bio = updatedUser.Bio,
            Email = updatedUser.Email,
            FirstName = updatedUser.FirstName,
            LastName = updatedUser.LastName,
            ProfileUrl = updatedUser.ProfileUrl,
            CreatedAt = updatedUser.CreatedAt
        };
    }
}