using Microsoft.Extensions.DependencyInjection;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Infrastructure.Services;

namespace SettingsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
