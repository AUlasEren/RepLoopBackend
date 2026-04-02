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

        var html = BuildEmailHtml(evt.DisplayName, evt.ResetCode);

        await _emailSender.SendAsync(
            evt.Email,
            "RepLoop — Şifre Sıfırlama Kodu",
            html,
            context.CancellationToken);
    }

    private static string BuildEmailHtml(string displayName, string resetCode) => $$"""
        <!DOCTYPE html>
        <html>
        <body style="font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; max-width: 600px; margin: 0 auto; padding: 0; background-color: #f5f5f5;">
          <div style="background-color: #ffffff; margin: 20px auto; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 8px rgba(0,0,0,0.08);">

            <!-- Header -->
            <div style="background: linear-gradient(135deg, #6366f1 0%, #8b5cf6 100%); padding: 32px 24px; text-align: center;">
              <h1 style="color: #ffffff; margin: 0; font-size: 24px; font-weight: 700;">RepLoop</h1>
              <p style="color: rgba(255,255,255,0.85); margin: 8px 0 0 0; font-size: 14px;">Şifre Sıfırlama</p>
            </div>

            <!-- Body -->
            <div style="padding: 32px 24px;">
              <p style="color: #1a1a2e; font-size: 16px; margin: 0 0 16px 0;">
                Merhaba <strong>{{displayName}}</strong>,
              </p>
              <p style="color: #4a4a68; font-size: 14px; line-height: 1.6; margin: 0 0 24px 0;">
                Şifrenizi sıfırlamak için aşağıdaki kodu uygulamaya girin. Bu kod <strong>60 dakika</strong> geçerlidir.
              </p>

              <!-- Code Box -->
              <div style="background: #f0f0ff; border: 2px dashed #6366f1; border-radius: 12px; padding: 24px; text-align: center; margin: 0 0 24px 0;">
                <p style="margin: 0 0 8px 0; color: #6366f1; font-size: 12px; font-weight: 600; text-transform: uppercase; letter-spacing: 2px;">Doğrulama Kodu</p>
                <p style="margin: 0; font-size: 36px; font-weight: 800; letter-spacing: 8px; color: #1a1a2e; font-family: 'Courier New', monospace;">{{resetCode}}</p>
              </div>

              <p style="color: #4a4a68; font-size: 13px; line-height: 1.6; margin: 0 0 8px 0;">
                Bu kodu kimseyle paylaşmayın. RepLoop ekibi sizden asla kod istemez.
              </p>
            </div>

            <!-- Footer -->
            <div style="background: #f8f8fc; padding: 16px 24px; border-top: 1px solid #e8e8f0;">
              <p style="color: #9898b0; font-size: 11px; margin: 0; text-align: center;">
                Eğer bu isteği siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz.
              </p>
            </div>
          </div>
        </body>
        </html>
        """;
}
