using MediatR;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Common;

namespace UserService.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly UsersManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateProfileCommandHandler(UsersManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken ct)
        => _manager.UpdateProfileAsync(request, _currentUser.UserId, ct);
}
