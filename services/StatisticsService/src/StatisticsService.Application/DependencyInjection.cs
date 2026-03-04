using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RepLoopBackend.SharedKernel.Behaviors;
using StatisticsService.Application.Features.Statistics;

namespace StatisticsService.Application;

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
        services.AddScoped<StatisticsManager>();

        return services;
    }
}
