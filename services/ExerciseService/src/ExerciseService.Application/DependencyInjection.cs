using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RepLoopBackend.SharedKernel.Behaviors;
using ExerciseService.Application.Features.Exercises;

namespace ExerciseService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });
        services.AddScoped<ExercisesManager>();

        return services;
    }
}
