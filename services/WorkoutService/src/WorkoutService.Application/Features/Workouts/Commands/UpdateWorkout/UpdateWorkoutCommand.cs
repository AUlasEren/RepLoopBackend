using MediatR;

namespace WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;

public record UpdateWorkoutCommand : IRequest
{
    public Guid UserId { get; init; }
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public DateTime? ScheduledDate { get; init; }
    public int? DurationMinutes { get; init; }
}
