using MediatR;

namespace SessionService.Application.Features.Sessions.Commands.UpdateSession;

public enum SessionAction { Pause, Resume }

public record UpdateSessionCommand : IRequest
{
    public Guid UserId { get; init; }
    public Guid Id { get; init; }
    public SessionAction Action { get; init; }
}