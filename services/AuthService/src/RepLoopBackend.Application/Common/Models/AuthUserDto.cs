namespace RepLoopBackend.Application.Common.Models;

public record AuthUserDto(
    Guid Id,
    string Name,
    string Email,
    string? AvatarUrl,
    bool IsProfileComplete);