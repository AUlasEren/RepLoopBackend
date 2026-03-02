using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<WorkoutDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IWorkoutDbContext>(p => p.GetRequiredService<WorkoutDbContext>());
        return services;
    }
}
