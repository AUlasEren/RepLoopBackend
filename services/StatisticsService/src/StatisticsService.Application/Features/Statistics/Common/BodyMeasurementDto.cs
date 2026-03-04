namespace StatisticsService.Application.Features.Statistics.Common;

public class BodyMeasurementDto
{
    public Guid Id { get; set; }
    public DateTime MeasuredAt { get; set; }
    public decimal? WeightKg { get; set; }
    public decimal? BodyFatPercentage { get; set; }
    public decimal? ChestCm { get; set; }
    public decimal? WaistCm { get; set; }
    public decimal? HipsCm { get; set; }
    public decimal? BicepsCm { get; set; }
    public decimal? ThighCm { get; set; }
    public string? Notes { get; set; }
}
