using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var (success, error, userInfo) = await _identityService.LoginAsync(request.Email, request.Password);

        if (!success || userInfo == null)
            throw new BadRequestException(ErrorCodes.LoginFailed, error ?? "Giriş başarısız.");

        var accessToken = _jwtTokenService.GenerateToken(userInfo.Id, userInfo.Email);
        var refreshToken = await _identityService.CreateRefreshTokenAsync(userInfo.Id);

        return new AuthResult(
            accessToken,
            refreshToken,
            new AuthUserDto(userInfo.Id, userInfo.DisplayName, userInfo.Email, userInfo.AvatarUrl, userInfo.IsProfileComplete));
    }
}
