using Server.Domain.Dto.Options;
using Server.Service.Interfaces;
using Server.Service.Services;

namespace Server.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IMonitorService, MonitorService>();
        services.AddScoped<ILogService, LogService>();

        return services;
    }
}