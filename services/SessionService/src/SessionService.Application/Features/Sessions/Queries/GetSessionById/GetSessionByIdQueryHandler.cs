using MediatR;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionById;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, SessionDto?>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetSessionByIdQueryHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SessionDto?> Handle(GetSessionByIdQuery request, CancellationToken ct)
        => _manager.GetSessionByIdAsync(request.Id, _currentUser.UserId, ct);
}
