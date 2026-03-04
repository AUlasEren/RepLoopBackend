using Microsoft.Extensions.DependencyInjection;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Infrastructure.Services;

namespace StatisticsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
