using MediatR;
using SessionService.Application.Common.Interfaces;

namespace SessionService.Application.Features.Sessions.Commands.StartSession;

public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand, Guid>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public StartSessionCommandHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(StartSessionCommand request, CancellationToken ct)
        => _manager.StartSessionAsync(request, _currentUser.UserId, ct);
}
