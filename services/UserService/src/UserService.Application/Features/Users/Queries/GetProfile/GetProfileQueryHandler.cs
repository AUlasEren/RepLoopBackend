using MediatR;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Common;

namespace UserService.Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly UsersManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetProfileQueryHandler(UsersManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
        => _manager.GetProfileAsync(_currentUser.UserId, ct);
}
