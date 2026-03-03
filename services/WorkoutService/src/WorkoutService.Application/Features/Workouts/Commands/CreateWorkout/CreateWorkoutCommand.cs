using MediatR;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public record CreateWorkoutCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public DateTime? ScheduledDate { get; init; }
    public int? DurationMinutes { get; init; }
    public List<CreateWorkoutExerciseItem> Exercises { get; init; } = new();
}
