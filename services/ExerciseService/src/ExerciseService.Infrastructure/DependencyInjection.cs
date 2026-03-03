using Microsoft.Extensions.DependencyInjection;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Infrastructure.Services;

namespace ExerciseService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }
}
