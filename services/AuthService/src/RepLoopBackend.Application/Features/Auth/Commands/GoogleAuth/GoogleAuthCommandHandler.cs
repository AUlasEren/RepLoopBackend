using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.GoogleAuth;

public class GoogleAuthCommandHandler : IRequestHandler<GoogleAuthCommand, AuthResult>
{
    private readonly IGoogleAuthService _googleAuthService;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public GoogleAuthCommandHandler(
        IGoogleAuthService googleAuthService,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _googleAuthService = googleAuthService;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(GoogleAuthCommand request, CancellationToken cancellationToken)
    {
        var googleUser = await _googleAuthService.VerifyTokenAsync(request.IdToken);

        if (googleUser == null)
            throw new BadRequestException(ErrorCodes.InvalidGoogleToken, "Geçersiz Google token.");

        var (success, error, userInfo) = await _identityService.LoginOrCreateOAuthUserAsync(
            googleUser.Email, googleUser.Name, googleUser.AvatarUrl);

        if (!success || userInfo == null)
            throw new BadRequestException(ErrorCodes.GoogleAuthFailed, error ?? "Google kimlik doğrulama başarısız.");

        var accessToken = _jwtTokenService.GenerateToken(userInfo.Id, userInfo.Email);
        var refreshToken = await _identityService.CreateRefreshTokenAsync(userInfo.Id);

        return new AuthResult(
            accessToken,
            refreshToken,
            new AuthUserDto(userInfo.Id, userInfo.DisplayName, userInfo.Email, userInfo.AvatarUrl, userInfo.IsProfileComplete));
    }
}
