using Server.Domain.Dto.Request;
using Server.Domain.Dto.Response;

namespace Server.Service.Interfaces;

public interface IAuthService
{
    Task<string> SignupTask(SignupRequest request);
    Task<string> LoginTask(LoginRequest request);
    Task GenerateOtp(string email);
    Task VerifyUser(VerifyRequest request);
    Task ResetUserPassword(ResetRequest request);
    Task<GetMeResponse> GetMeTask(Guid userId);
}