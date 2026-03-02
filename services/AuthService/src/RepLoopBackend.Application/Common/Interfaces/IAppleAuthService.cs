namespace RepLoopBackend.Application.Common.Interfaces;

public record AppleUserInfo(string Email, string Name);

public interface IAppleAuthService
{
    Task<AppleUserInfo?> VerifyTokenAsync(string identityToken, string? fullName = null);
}