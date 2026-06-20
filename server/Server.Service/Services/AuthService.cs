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

        await emailService.SendEmailAsync(
    user.Email,
    "🎉 Welcome to Pulse Monitor!",
    $"""
    <html>
    <body style="margin:0;padding:0;background-color:#f5f5f5;font-family:Arial,Helvetica,sans-serif;">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" style="padding:40px 20px;">
                    <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:10px;padding:40px;box-shadow:0 2px 8px rgba(0,0,0,0.08);">

                        <tr>
                            <td align="center">
                                <h1 style="color:#2563eb;margin-bottom:10px;">
                                    Welcome to Pulse Monitor!
                                </h1>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <p style="font-size:16px;color:#333;">
                                    Hi <strong>{user.FirstName}</strong>,
                                </p>

                                <p style="font-size:16px;color:#333;line-height:1.8;">
                                    Thank you for joining <strong>Pulse Monitor</strong>.
                                    Your account has been created successfully and you're ready
                                    to start monitoring your APIs, websites, and services.
                                </p>

                                <hr style="margin:30px 0;border:none;border-top:1px solid #e5e5e5;" />

                                <h3 style="color:#2563eb;">
                                    What you can do next
                                </h3>

                                <ul style="color:#333;line-height:1.8;">
                                    <li>Create your first monitor.</li>
                                    <li>Choose the monitoring interval.</li>
                                    <li>Receive instant email alerts when an endpoint goes down.</li>
                                    <li>View response times and status history.</li>
                                    <li>Track uptime with detailed monitoring logs.</li>
                                </ul>

                                <hr style="margin:30px 0;border:none;border-top:1px solid #e5e5e5;" />

                                <p style="font-size:16px;color:#333;">
                                    We hope Pulse Monitor helps you keep your services reliable and available.
                                </p>

                                <p style="font-size:16px;color:#333;">
                                    Happy Monitoring!
                                </p>

                                <br />

                                <p style="color:#2563eb;font-weight:bold;">
                                    — Pulse Monitor Team
                                </p>

                                <br />

                                <p style="font-size:12px;color:#888;">
                                    This is an automated email. Please do not reply.
                                </p>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    """);

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

        await emailService.SendEmailAsync(
    email,
    "🔐 Pulse Monitor - Password Reset OTP",
    $"""
    <html>
    <body style="margin:0;padding:0;background-color:#f5f5f5;font-family:Arial,Helvetica,sans-serif;">
        <table width="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" style="padding:40px 20px;">
                    <table width="600" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:10px;padding:40px;box-shadow:0 2px 8px rgba(0,0,0,0.08);">

                        <tr>
                            <td align="center">
                                <h1 style="color:#2563eb;margin-bottom:10px;">
                                    Password Reset Request
                                </h1>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <p style="font-size:16px;color:#333;">
                                    We received a request to reset the password for your
                                    <strong>Pulse Monitor</strong> account.
                                </p>

                                <p style="font-size:16px;color:#333;">
                                    Use the following One-Time Password (OTP) to continue:
                                </p>

                                <div style="margin:35px 0;text-align:center;">
                                    <span style="
                                        display:inline-block;
                                        padding:16px 32px;
                                        font-size:32px;
                                        font-weight:bold;
                                        letter-spacing:8px;
                                        color:#2563eb;
                                        background:#eef4ff;
                                        border:2px dashed #2563eb;
                                        border-radius:8px;">
                                        {user.Otp}
                                    </span>
                                </div>

                                <p style="font-size:15px;color:#333;">
                                    This OTP is valid for <strong>10 minutes</strong>.
                                </p>

                                <p style="font-size:15px;color:#333;">
                                    If you did not request a password reset, you can safely ignore this email. Your account will remain secure.
                                </p>

                                <hr style="margin:30px 0;border:none;border-top:1px solid #e5e5e5;" />

                                <p style="font-size:13px;color:#777;">
                                    For security reasons, never share this OTP with anyone.
                                    Pulse Monitor will never ask you for your OTP via email or phone.
                                </p>

                                <br />

                                <p style="color:#2563eb;font-weight:bold;">
                                    — Pulse Monitor Team
                                </p>

                                <p style="font-size:12px;color:#888;">
                                    This is an automated email. Please do not reply.
                                </p>
                            </td>
                        </tr>

                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    """);
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
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Value.Issuer,
            audience: jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}