using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Infrastructure.Messaging;
using RepLoopBackend.Infrastructure.Services;

namespace RepLoopBackend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IAppleAuthService, AppleAuthService>();
        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddHttpClient();

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(
                    configuration["RabbitMQ:Host"] ?? "localhost",
                    configuration["RabbitMQ:VHost"] ?? "/",
                    h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"] ?? "reploop");
                        h.Password(configuration["RabbitMQ:Password"] ?? "reploop123");
                    });

                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}
