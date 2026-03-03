using MediatR;

namespace SessionService.Application.Features.Sessions.Commands.LogSet;

public record LogSetCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public Guid SessionId { get; init; }
    public Guid ExerciseId { get; init; }
    public string ExerciseName { get; init; } = string.Empty;
    public int SetNumber { get; init; }
    public int? Reps { get; init; }
    public decimal? WeightKg { get; init; }
    public int? DurationSeconds { get; init; }
    public string? Notes { get; init; }
}
