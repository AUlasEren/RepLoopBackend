using Microsoft.Extensions.Logging;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public Task SendPasswordResetEmailAsync(string email, string resetToken)
    {
        // TODO: SendGrid / SMTP ile gerçek email gönderimi yapılacak
        _logger.LogInformation(
            "Şifre sıfırlama emaili gönderilecek: {Email} | Token: {Token}",
            email, resetToken);

        return Task.CompletedTask;
    }
}