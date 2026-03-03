using SessionService.Domain.Common;

namespace SessionService.Domain.Entities;

public class SessionSet : BaseEntity
{
    public Guid SessionId { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public int SetNumber { get; set; }
    public int? Reps { get; set; }
    public decimal? WeightKg { get; set; }
    public int? DurationSeconds { get; set; }
    public string? Notes { get; set; }
    public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

    public WorkoutSession Session { get; set; } = null!;
}
