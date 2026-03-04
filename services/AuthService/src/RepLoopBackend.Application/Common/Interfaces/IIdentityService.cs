using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<(bool Success, string? Error, UserInfo? User)> RegisterAsync(string email, string password, string displayName);
    Task<(bool Success, string? Error, UserInfo? User)> LoginAsync(string email, string password);
    Task<(bool Success, string? Error, UserInfo? User)> LoginOrCreateOAuthUserAsync(string email, string name, string? avatarUrl);
    Task<string> CreateRefreshTokenAsync(Guid userId);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task<UserInfo?> ValidateRefreshTokenAsync(string refreshToken);
    Task<(bool Found, string? ResetToken)> GeneratePasswordResetTokenAsync(string email);
    Task<(bool Success, string? Error)> ResetPasswordAsync(string email, string token, string newPassword);
    Task<(bool Success, string? Error)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}