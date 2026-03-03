namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public record CreateWorkoutExerciseItem
{
    public Guid ExerciseId { get; init; }
    public string ExerciseName { get; init; } = string.Empty;
    public int Sets { get; init; }
    public int Reps { get; init; }
    public decimal? WeightKg { get; init; }
    public int? DurationSeconds { get; init; }
    public string? Notes { get; init; }
}
