using Server.Domain.Interfaces.Infrastructure;
using Server.Infrastructure.Services;

namespace Server.Api.Extensions;

public static class InfrastructureCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMonitorChecker, MonitorChecker>();

        return services;
    }
}