using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Infrastructure.Services;

namespace WorkoutService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
