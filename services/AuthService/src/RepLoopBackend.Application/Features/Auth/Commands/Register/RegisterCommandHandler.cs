using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResult>
{
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public RegisterCommandHandler(IIdentityService identityService, IJwtTokenService jwtTokenService)
    {
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var (success, error, userInfo) = await _identityService.RegisterAsync(request.Email, request.Password, request.DisplayName);

        if (!success || userInfo == null)
            throw new BadRequestException(ErrorCodes.RegistrationFailed, error ?? "Kayıt başarısız.");

        var accessToken = _jwtTokenService.GenerateToken(userInfo.Id, userInfo.Email);
        var refreshToken = await _identityService.CreateRefreshTokenAsync(userInfo.Id);

        return new AuthResult(
            accessToken,
            refreshToken,
            new AuthUserDto(userInfo.Id, userInfo.DisplayName, userInfo.Email, userInfo.AvatarUrl, userInfo.IsProfileComplete));
    }
}