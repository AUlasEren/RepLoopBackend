using MediatR;
using SessionService.Application.Common.Interfaces;

namespace SessionService.Application.Features.Sessions.Commands.LogSet;

public class LogSetCommandHandler : IRequestHandler<LogSetCommand, Guid>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public LogSetCommandHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(LogSetCommand request, CancellationToken ct)
        => _manager.LogSetAsync(request, _currentUser.UserId, ct);
}
