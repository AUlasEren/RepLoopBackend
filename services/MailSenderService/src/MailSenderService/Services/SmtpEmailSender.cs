using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MailSenderService.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var smtp = _configuration.GetSection("Smtp");

        var host     = smtp["Host"]     ?? "smtp.resend.com";
        var port     = int.Parse(smtp["Port"] ?? "465");
        var from     = smtp["From"]     ?? "onboarding@resend.dev";
        var username = smtp["Username"] ?? "resend";
        var password = smtp["Password"] ?? string.Empty;

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(from));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (_, _, _, _) => true;

        await client.ConnectAsync(host, port, SecureSocketOptions.Auto, cancellationToken);

        if (!string.IsNullOrEmpty(password))
            await client.AuthenticateAsync(username, password, cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);

        _logger.LogInformation("Email gönderildi → {To} | {Subject}", to, subject);
    }
}
