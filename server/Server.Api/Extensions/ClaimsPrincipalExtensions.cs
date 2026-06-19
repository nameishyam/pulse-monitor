using System.Security.Claims;

namespace Server.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        return string.IsNullOrWhiteSpace(userId) 
            ? throw new UnauthorizedAccessException("User ID claim not found.") 
            : Guid.Parse(userId);
    }
}