namespace RepLoopBackend.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetToken);
}