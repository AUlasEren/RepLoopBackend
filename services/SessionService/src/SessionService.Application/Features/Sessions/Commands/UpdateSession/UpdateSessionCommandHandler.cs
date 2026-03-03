using MediatR;
using SessionService.Application.Common.Interfaces;

namespace SessionService.Application.Features.Sessions.Commands.UpdateSession;

public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand>
{
    private readonly SessionsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateSessionCommandHandler(SessionsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(UpdateSessionCommand request, CancellationToken ct)
        => _manager.UpdateSessionAsync(request, _currentUser.UserId, ct);
}
