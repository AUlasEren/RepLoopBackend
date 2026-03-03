using Microsoft.Extensions.DependencyInjection;
using SessionService.Application.Common.Interfaces;
using SessionService.Infrastructure.Services;

namespace SessionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
