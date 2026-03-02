using WorkoutService.Domain.Common;

namespace WorkoutService.Domain.Entities;

public class WorkoutExercise : BaseEntity
{
    public Guid WorkoutId { get; set; }
    public Workout Workout { get; set; } = null!;
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal? WeightKg { get; set; }
    public int? DurationSeconds { get; set; }
    public string? Notes { get; set; }
}
