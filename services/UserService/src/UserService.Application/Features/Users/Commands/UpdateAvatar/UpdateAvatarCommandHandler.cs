using MediatR;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Features.Users.Commands.UpdateAvatar;

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, string>
{
    private readonly UsersManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateAvatarCommandHandler(UsersManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<string> Handle(UpdateAvatarCommand request, CancellationToken ct)
        => _manager.UpdateAvatarAsync(request, _currentUser.UserId, ct);
}
