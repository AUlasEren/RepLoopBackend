using MediatR;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Features.Users.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly UsersManager _manager;
    private readonly ICurrentUserService _currentUser;

    public DeleteAccountCommandHandler(UsersManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(DeleteAccountCommand request, CancellationToken ct)
        => _manager.DeleteAccountAsync(_currentUser.UserId, ct);
}
