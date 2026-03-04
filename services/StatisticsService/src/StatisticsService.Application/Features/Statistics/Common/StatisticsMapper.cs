using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Features.Statistics.Common;

public static class StatisticsMapper
{
    public static BodyMeasurementDto ToDto(this BodyMeasurement m) => new()
    {
        Id = m.Id,
        MeasuredAt = m.MeasuredAt,
        WeightKg = m.WeightKg,
        BodyFatPercentage = m.BodyFatPercentage,
        ChestCm = m.ChestCm,
        WaistCm = m.WaistCm,
        HipsCm = m.HipsCm,
        BicepsCm = m.BicepsCm,
        ThighCm = m.ThighCm,
        Notes = m.Notes
    };

    public static ExerciseLogDto ToDto(this ExerciseLog l) => new()
    {
        Id = l.Id,
        ExerciseId = l.ExerciseId,
        ExerciseName = l.ExerciseName,
        WeightKg = l.WeightKg,
        Reps = l.Reps,
        PerformedAt = l.PerformedAt
    };
}
