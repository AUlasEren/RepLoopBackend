using MediatR;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly AuthManager _manager;
    private readonly ICurrentUserService _currentUser;

    public ChangePasswordCommandHandler(AuthManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(ChangePasswordCommand request, CancellationToken ct)
        => _manager.ChangePasswordAsync(request, _currentUser.UserId, ct);
}
