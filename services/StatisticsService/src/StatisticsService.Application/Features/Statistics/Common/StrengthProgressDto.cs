namespace StatisticsService.Application.Features.Statistics.Common;

public class StrengthProgressDto
{
    public Guid ExerciseId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public List<StrengthDataPoint> DataPoints { get; set; } = new();
}

public class StrengthDataPoint
{
    public DateTime Date { get; set; }
    public decimal MaxWeightKg { get; set; }
    public int MaxReps { get; set; }
    public decimal TotalVolume { get; set; }
}
