using MediatR;
using SessionService.Application.Common.Interfaces;

namespace SessionService.Application.Features.Sessions.Commands.AbandonSession;

public class AbandonSessionCommandHandler : IRequestHandler<AbandonSessionCommand>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public AbandonSessionCommandHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(AbandonSessionCommand request, CancellationToken ct)
        => _manager.AbandonSessionAsync(request, _currentUser.UserId, ct);
}