using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.AppleAuth;

public class AppleAuthCommandHandler : IRequestHandler<AppleAuthCommand, AuthResult>
{
    private readonly IAppleAuthService _appleAuthService;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public AppleAuthCommandHandler(
        IAppleAuthService appleAuthService,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _appleAuthService = appleAuthService;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(AppleAuthCommand request, CancellationToken cancellationToken)
    {
        var appleUser = await _appleAuthService.VerifyTokenAsync(request.IdentityToken, request.FullName);

        if (appleUser == null)
            throw new BadRequestException(ErrorCodes.InvalidAppleToken, "Geçersiz Apple token.");

        var (success, error, userInfo) = await _identityService.LoginOrCreateOAuthUserAsync(
            appleUser.Email, appleUser.Name, avatarUrl: null);

        if (!success || userInfo == null)
            throw new BadRequestException(ErrorCodes.AppleAuthFailed, error ?? "Apple kimlik doğrulama başarısız.");

        var accessToken = _jwtTokenService.GenerateToken(userInfo.Id, userInfo.Email);
        var refreshToken = await _identityService.CreateRefreshTokenAsync(userInfo.Id);

        return new AuthResult(
            accessToken,
            refreshToken,
            new AuthUserDto(userInfo.Id, userInfo.DisplayName, userInfo.Email, userInfo.AvatarUrl, userInfo.IsProfileComplete));
    }
}