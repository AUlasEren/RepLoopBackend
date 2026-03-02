namespace RepLoopBackend.Application.Common.Models;

public record UserInfo(
    Guid Id,
    string Email,
    string DisplayName,
    string? AvatarUrl,
    bool IsProfileComplete);