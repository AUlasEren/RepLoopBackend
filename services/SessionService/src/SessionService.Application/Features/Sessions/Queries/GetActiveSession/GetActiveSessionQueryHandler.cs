using MediatR;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetActiveSession;

public class GetActiveSessionQueryHandler : IRequestHandler<GetActiveSessionQuery, SessionDto?>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetActiveSessionQueryHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SessionDto?> Handle(GetActiveSessionQuery request, CancellationToken ct)
        => _manager.GetActiveSessionAsync(_currentUser.UserId, ct);
}
