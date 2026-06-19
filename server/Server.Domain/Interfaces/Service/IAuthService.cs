using Server.Domain.Dto.Request.Auth;
using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface IAuthService
{
    Task<string> SignupTask(SignupRequest request);
    Task<string> LoginTask(LoginRequest request);
    Task GenerateOtp(string email);
    Task VerifyUser(VerifyRequest request);
    Task ResetUserPassword(ResetRequest request);
    Task<GetMeResponse> GetMeTask(Guid userId);
}