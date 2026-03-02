using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ExerciseDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IExerciseDbContext>(p => p.GetRequiredService<ExerciseDbContext>());
        return services;
    }
}
