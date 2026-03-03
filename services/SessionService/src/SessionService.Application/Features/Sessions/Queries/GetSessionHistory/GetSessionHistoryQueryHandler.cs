using MediatR;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionHistory;

public class GetSessionHistoryQueryHandler : IRequestHandler<GetSessionHistoryQuery, SessionHistoryDto>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetSessionHistoryQueryHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SessionHistoryDto> Handle(GetSessionHistoryQuery request, CancellationToken ct)
        => _manager.GetSessionHistoryAsync(_currentUser.UserId, request.Page, request.PageSize, ct);
}
