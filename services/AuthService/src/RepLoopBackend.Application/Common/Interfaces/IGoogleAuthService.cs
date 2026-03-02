namespace RepLoopBackend.Application.Common.Interfaces;

public record GoogleUserInfo(string Email, string Name, string? AvatarUrl);

public interface IGoogleAuthService
{
    Task<GoogleUserInfo?> VerifyTokenAsync(string idToken);
}