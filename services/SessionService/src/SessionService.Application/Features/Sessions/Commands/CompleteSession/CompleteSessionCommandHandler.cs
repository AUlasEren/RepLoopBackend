using MediatR;
using SessionService.Application.Common.Interfaces;

namespace SessionService.Application.Features.Sessions.Commands.CompleteSession;

public class CompleteSessionCommandHandler : IRequestHandler<CompleteSessionCommand>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public CompleteSessionCommandHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(CompleteSessionCommand request, CancellationToken ct)
        => _manager.CompleteSessionAsync(request, _currentUser.UserId, ct);
}
