using MassTransit;
using MailSenderService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepLoopBackend.Contracts.Events;

namespace MailSenderService.Consumers;

public class PasswordResetConsumer : IConsumer<PasswordResetRequestedEvent>
{
    private readonly IEmailSender _emailSender;
    private readonly IConfiguration _configuration;
    private readonly ILogger<PasswordResetConsumer> _logger;

    public PasswordResetConsumer(
        IEmailSender emailSender,
        IConfiguration configuration,
        ILogger<PasswordResetConsumer> logger)
    {
        _emailSender = emailSender;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PasswordResetRequestedEvent> context)
    {
        var evt = context.Message;
        _logger.LogInformation("Şifre sıfırlama eventi alındı: {Email}", evt.Email);

        var html = BuildEmailHtml(evt.DisplayName, evt.Email, evt.ResetToken);

        await _emailSender.SendAsync(
            evt.Email,
            "RepLoop — Şifre Sıfırlama",
            html,
            context.CancellationToken);
    }

    private static string BuildEmailHtml(string displayName, string email, string resetToken) => $$"""
        <!DOCTYPE html>
        <html>
        <body style="font-family: sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;">
          <h2>Merhaba {{displayName}},</h2>
          <p>Şifrenizi sıfırlamak için aşağıdaki bilgileri kullanın.</p>
          <p><strong>Email:</strong> {{email}}</p>
          <div style="background:#f4f4f8; border:1px solid #d1d5db; border-radius:8px; padding:16px; margin:16px 0;">
            <p style="margin:0 0 8px 0; font-weight:bold;">Sıfırlama Token'ı:</p>
            <code style="word-break:break-all; font-size:13px;">{{resetToken}}</code>
          </div>
          <p>Bu token'ı <strong>POST /api/auth/reset-password</strong> endpoint'ine gönderin:</p>
          <pre style="background:#1e1e2e; color:#cdd6f4; padding:16px; border-radius:8px; font-size:12px; overflow-x:auto;">
        {
          "email": "{{email}}",
          "token": "&lt;yukarıdaki token&gt;",
          "newPassword": "YeniSifreniz123"
        }</pre>
          <p style="color:#888; font-size:12px; margin-top:24px;">
            Bu token 60 dakika geçerlidir. Eğer bu isteği siz yapmadıysanız bu emaili görmezden gelin.
          </p>
        </body>
        </html>
        """;
}
