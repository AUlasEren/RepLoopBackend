using MediatR;

namespace StatisticsService.Application.Features.Statistics.Commands.LogExercise;

public record LogExerciseCommand : IRequest<Guid>
{
    public Guid ExerciseId { get; init; }
    public string ExerciseName { get; init; } = string.Empty;
    public decimal WeightKg { get; init; }
    public int Reps { get; init; }
    public DateTime PerformedAt { get; init; }
}
