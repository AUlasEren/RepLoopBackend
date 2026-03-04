namespace StatisticsService.Application.Features.Statistics.Common;

public class ExerciseLogDto
{
    public Guid Id { get; set; }
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public int Reps { get; set; }
    public DateTime PerformedAt { get; set; }
}
