namespace RepLoopBackend.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(Guid userId, string email);
}
