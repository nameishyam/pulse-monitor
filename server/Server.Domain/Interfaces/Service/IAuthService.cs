using Server.Domain.Dto.Request.Auth;
using Server.Domain.Dto.Response;

namespace Server.Domain.Interfaces.Service;

public interface IAuthService
{
    Task<string> SignupTask(UserSignup request);
    Task<string> LoginTask(UserLogin request);
    Task GenerateOtp(string email);
    Task VerifyUser(VerifyOtp request);
    Task ResetUserPassword(ResetPassword request);
    Task<GetMe> GetMeTask(Guid userId);
}