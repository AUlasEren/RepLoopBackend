using Microsoft.EntityFrameworkCore;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Application.Common.Interfaces;

public interface IStatisticsDbContext
{
    DbSet<BodyMeasurement> BodyMeasurements { get; }
    DbSet<ExerciseLog> ExerciseLogs { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
