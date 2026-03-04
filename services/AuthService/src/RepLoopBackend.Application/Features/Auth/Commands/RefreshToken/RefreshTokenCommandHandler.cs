using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userInfo = await _identityService.ValidateRefreshTokenAsync(request.RefreshToken);

        if (userInfo == null)
            throw new BadRequestException(ErrorCodes.InvalidRefreshToken, "Geçersiz veya süresi dolmuş refresh token.");

        await _identityService.RevokeRefreshTokenAsync(request.RefreshToken);

        var accessToken = _jwtTokenService.GenerateToken(userInfo.Id, userInfo.Email);
        var newRefreshToken = await _identityService.CreateRefreshTokenAsync(userInfo.Id);

        return new AuthResult(
            accessToken,
            newRefreshToken,
            new AuthUserDto(userInfo.Id, userInfo.DisplayName, userInfo.Email, userInfo.AvatarUrl, userInfo.IsProfileComplete));
    }
}
