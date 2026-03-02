namespace RepLoopBackend.Application.Common.Models;

public record AuthResult(string Token, string RefreshToken, AuthUserDto User);