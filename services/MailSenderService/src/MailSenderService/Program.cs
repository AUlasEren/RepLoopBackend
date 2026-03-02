using MassTransit;
using MailSenderService.Consumers;
using MailSenderService.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PasswordResetConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        var rabbitMq = builder.Configuration.GetSection("RabbitMQ");

        cfg.Host(
            rabbitMq["Host"] ?? "localhost",
            rabbitMq["VHost"] ?? "/",
            h =>
            {
                h.Username(rabbitMq["Username"] ?? "reploop");
                h.Password(rabbitMq["Password"] ?? "reploop123");
            });

        cfg.ConfigureEndpoints(ctx);
    });
});

var host = builder.Build();
host.Run();