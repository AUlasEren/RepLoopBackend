namespace StatisticsService.Application.Features.Statistics.Common;

public class PersonalRecordDto
{
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public decimal MaxWeightKg { get; set; }
    public int MaxReps { get; set; }
    public DateTime AchievedAt { get; set; }
}
