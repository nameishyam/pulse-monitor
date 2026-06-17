using Microsoft.EntityFrameworkCore;
using Server.Repository.Context;
using Server.Repository.Interfaces;
using Server.Repository.Repositories;

namespace Server.Api.Extensions;

public static class RepositoryCollectionExtensions
{
    public static IServiceCollection AddRepository(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ServerDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IMonitorRepository, MonitorRepository>(); 
        services.AddScoped<ILogRepository, LogRepository>();

        return services;
    }
}