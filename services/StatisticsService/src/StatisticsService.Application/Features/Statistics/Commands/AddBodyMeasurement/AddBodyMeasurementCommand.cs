using MediatR;

namespace StatisticsService.Application.Features.Statistics.Commands.AddBodyMeasurement;

public record AddBodyMeasurementCommand : IRequest<Guid>
{
    public DateTime MeasuredAt { get; init; }
    public decimal? WeightKg { get; init; }
    public decimal? BodyFatPercentage { get; init; }
    public decimal? ChestCm { get; init; }
    public decimal? WaistCm { get; init; }
    public decimal? HipsCm { get; init; }
    public decimal? BicepsCm { get; init; }
    public decimal? ThighCm { get; init; }
    public string? Notes { get; init; }
}
