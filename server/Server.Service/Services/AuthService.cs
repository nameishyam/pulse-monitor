using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Domain.Dto.Options;
using Server.Domain.Dto.Request.Auth;
using Server.Domain.Dto.Response;
using Server.Domain.Entities;
using Server.Domain.Interfaces.Repository;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;

namespace Server.Service.Services;

public class AuthService(
    IAuthRepository authRepository,
    IOptions<JwtOptions> jwtOptions,
    IEmailService emailService)
    : IAuthService
{
    public async Task<string> SignupTask(UserSignup request)
    {
        if (await authRepository.ExistsByEmail(request.Email))
        {
            throw new ConflictException($"user with the email {request.Email} already exists");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await authRepository.Create(user);

        await emailService.SendEmailAsync(user.Email, "Welcome to Pulse Monitor",
            $"Glad to have you onboard {user.FirstName}!");

        return GenerateToken(user);
    }

    public async Task<string> LoginTask(UserLogin request)
    {
        if (!await authRepository.ExistsByEmail(request.Email))
        {
            throw new NotFoundException($"user with the email {request.Email} not exists");
        }

        var user = await authRepository.GetByEmail(request.Email);

        return !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)
            ? throw new InvalidDetailsException("user password didn't match, try again")
            : GenerateToken(user);
    }

    public async Task GenerateOtp(string email)
    {
        var user = await authRepository.GetByEmail(email);

        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found");
        }

        user.Otp = RandomNumberGenerator.GetInt32(100000, 1000000);
        user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);
        await authRepository.Update(user);

        await emailService.SendEmailAsync(email, "Password reset OTP",
            $"Your OTP to reset password: {user.Otp}");
    }

    public async Task VerifyUser(VerifyOtp request)
    {
        var user = await authRepository.GetByEmail(request.Email);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (user.Otp != request.Otp)
        {
            throw new ForbidException("OTP didn't match");
        }

        if (user.OtpExpiry < DateTime.UtcNow)
        {
            throw new ForbidException("OTP expired");
        }

        await authRepository.ClearOtp(user.Id);
    }

    public async Task ResetUserPassword(ResetPassword request)
    {
        var user = await authRepository.GetByEmail(request.Email);

        user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await authRepository.Update(user);
    }

    public async Task<GetMe> GetMeTask(Guid userId)
    {
        var user = await authRepository.GetById(userId);

        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return new GetMe
        {
            User = new GetUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            },
            Monitors = user.Monitors
                .Select(m => new GetMonitor
                {
                    Id = m.Id,
                    Name = m.Name,
                    Url = m.Url,
                    IntervalSeconds = m.IntervalSeconds,
                    MonitorStatus = m.MonitorStatus,
                    LastChecked = m.LastChecked
                })
                .ToList()
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Value.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}