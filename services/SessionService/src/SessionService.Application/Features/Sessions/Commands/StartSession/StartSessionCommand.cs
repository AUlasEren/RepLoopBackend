using MediatR;

namespace SessionService.Application.Features.Sessions.Commands.StartSession;

public record StartSessionCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public Guid WorkoutId { get; init; }
    public string WorkoutName { get; init; } = string.Empty;
}