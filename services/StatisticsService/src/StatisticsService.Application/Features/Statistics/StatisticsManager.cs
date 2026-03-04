using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Application.Features.Statistics.Commands.AddBodyMeasurement;
using StatisticsService.Application.Features.Statistics.Commands.LogExercise;
using StatisticsService.Application.Features.Statistics.Commands.UpdateBodyMeasurement;
using StatisticsService.Application.Features.Statistics.Common;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Features.Statistics;

public class StatisticsManager
{
    private readonly IStatisticsDbContext _context;

    public StatisticsManager(IStatisticsDbContext context)
    {
        _context = context;
    }

    public async Task<StrengthProgressDto> GetStrengthProgressAsync(
        Guid userId, Guid exerciseId, string period, CancellationToken ct)
    {
        var days = ParsePeriodToDays(period);
        var since = DateTime.UtcNow.AddDays(-days);

        var logs = await _context.ExerciseLogs
            .Where(l => l.UserId == userId && l.ExerciseId == exerciseId && l.PerformedAt >= since)
            .OrderBy(l => l.PerformedAt)
            .ToListAsync(ct);

        var exerciseName = logs.FirstOrDefault()?.ExerciseName ?? string.Empty;

        var dataPoints = logs
            .GroupBy(l => l.PerformedAt.Date)
            .Select(g => new StrengthDataPoint
            {
                Date = g.Key,
                MaxWeightKg = g.Max(l => l.WeightKg),
                MaxReps = g.Max(l => l.Reps),
                TotalVolume = g.Sum(l => l.WeightKg * l.Reps)
            })
            .ToList();

        return new StrengthProgressDto
        {
            ExerciseId = exerciseId,
            ExerciseName = exerciseName,
            DataPoints = dataPoints
        };
    }

    public async Task<List<PersonalRecordDto>> GetPersonalRecordsAsync(Guid userId, CancellationToken ct)
    {
        var records = await _context.ExerciseLogs
            .Where(l => l.UserId == userId)
            .GroupBy(l => new { l.ExerciseId, l.ExerciseName })
            .Select(g => new PersonalRecordDto
            {
                ExerciseId = g.Key.ExerciseId,
                ExerciseName = g.Key.ExerciseName,
                MaxWeightKg = g.Max(l => l.WeightKg),
                MaxReps = g.Max(l => l.Reps),
                AchievedAt = g.OrderByDescending(l => l.WeightKg).First().PerformedAt
            })
            .ToListAsync(ct);

        return records;
    }

    public async Task<PaginatedResult<BodyMeasurementDto>> GetBodyMeasurementsAsync(
        Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.BodyMeasurements
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.MeasuredAt);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PaginatedResult<BodyMeasurementDto>
        {
            Items = items.Select(m => m.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<Guid> AddBodyMeasurementAsync(
        AddBodyMeasurementCommand command, Guid userId, CancellationToken ct)
    {
        var measurement = new BodyMeasurement
        {
            UserId = userId,
            MeasuredAt = command.MeasuredAt,
            WeightKg = command.WeightKg,
            BodyFatPercentage = command.BodyFatPercentage,
            ChestCm = command.ChestCm,
            WaistCm = command.WaistCm,
            HipsCm = command.HipsCm,
            BicepsCm = command.BicepsCm,
            ThighCm = command.ThighCm,
            Notes = command.Notes
        };

        _context.BodyMeasurements.Add(measurement);
        await _context.SaveChangesAsync(ct);
        return measurement.Id;
    }

    public async Task DeleteBodyMeasurementAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var measurement = await _context.BodyMeasurements
            .FirstOrDefaultAsync(m => m.Id == id, ct)
            ?? throw new NotFoundException(ErrorCodes.MeasurementNotFound, "BodyMeasurement", id);

        if (measurement.UserId != userId)
            throw new ForbiddenException(ErrorCodes.MeasurementForbidden);

        _context.BodyMeasurements.Remove(measurement);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<BodyMeasurementDto> UpdateBodyMeasurementAsync(
        UpdateBodyMeasurementCommand command, Guid userId, CancellationToken ct)
    {
        var measurement = await _context.BodyMeasurements
            .FirstOrDefaultAsync(m => m.Id == command.Id, ct)
            ?? throw new NotFoundException(ErrorCodes.MeasurementNotFound, "BodyMeasurement", command.Id);

        if (measurement.UserId != userId)
            throw new ForbiddenException(ErrorCodes.MeasurementForbidden);

        measurement.MeasuredAt = command.MeasuredAt;
        measurement.WeightKg = command.WeightKg;
        measurement.BodyFatPercentage = command.BodyFatPercentage;
        measurement.ChestCm = command.ChestCm;
        measurement.WaistCm = command.WaistCm;
        measurement.HipsCm = command.HipsCm;
        measurement.BicepsCm = command.BicepsCm;
        measurement.ThighCm = command.ThighCm;
        measurement.Notes = command.Notes;
        measurement.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return measurement.ToDto();
    }

    public async Task<Guid> LogExerciseAsync(
        LogExerciseCommand command, Guid userId, CancellationToken ct)
    {
        var log = new ExerciseLog
        {
            UserId = userId,
            ExerciseId = command.ExerciseId,
            ExerciseName = command.ExerciseName,
            WeightKg = command.WeightKg,
            Reps = command.Reps,
            PerformedAt = command.PerformedAt
        };

        _context.ExerciseLogs.Add(log);
        await _context.SaveChangesAsync(ct);
        return log.Id;
    }

    private static int ParsePeriodToDays(string period)
    {
        if (string.IsNullOrWhiteSpace(period))
            return 30;

        var span = period.Trim().ToLowerInvariant();

        if (span.EndsWith("d") && int.TryParse(span[..^1], out var days))
            return days > 0 ? days : 30;

        if (span.EndsWith("w") && int.TryParse(span[..^1], out var weeks))
            return weeks > 0 ? weeks * 7 : 30;

        if (span.EndsWith("m") && int.TryParse(span[..^1], out var months))
            return months > 0 ? months * 30 : 30;

        throw new BadRequestException(ErrorCodes.InvalidPeriod,
            "Geçersiz period formatı. Örnekler: 7d, 4w, 3m");
    }
}
