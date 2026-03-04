using MediatR;

namespace WorkoutService.Application.Features.Workouts.Commands.DuplicateWorkout;

public record DuplicateWorkoutCommand : IRequest<Guid>
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
}
