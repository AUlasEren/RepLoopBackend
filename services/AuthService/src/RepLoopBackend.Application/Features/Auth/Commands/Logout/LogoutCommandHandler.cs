using MediatR;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IIdentityService _identityService;

    public LogoutCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        await _identityService.RevokeRefreshTokenAsync(request.RefreshToken);
    }
}
