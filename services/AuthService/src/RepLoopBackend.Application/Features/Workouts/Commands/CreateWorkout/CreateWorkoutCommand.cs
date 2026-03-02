using MediatR;

namespace RepLoopBackend.Application.Features.Workouts.Commands.CreateWorkout;

public record CreateWorkoutCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public List<Guid> ExerciseIds { get; init; } = new();
}
