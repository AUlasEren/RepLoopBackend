using StatisticsService.Domain.Common;

namespace StatisticsService.Domain.Entities;

public class ExerciseLog : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public DateTime PerformedAt { get; set; }
}
