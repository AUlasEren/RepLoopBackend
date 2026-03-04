using Microsoft.EntityFrameworkCore;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Domain.Entities;

namespace StatisticsService.Persistence;

public class StatisticsDbContext : DbContext, IStatisticsDbContext
{
    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : base(options) { }

    public DbSet<BodyMeasurement> BodyMeasurements => Set<BodyMeasurement>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<BodyMeasurement>(e =>
        {
            e.Property(m => m.WeightKg).HasColumnType("numeric(10,2)");
            e.Property(m => m.BodyFatPercentage).HasColumnType("numeric(5,2)");
            e.Property(m => m.ChestCm).HasColumnType("numeric(10,2)");
            e.Property(m => m.WaistCm).HasColumnType("numeric(10,2)");
            e.Property(m => m.HipsCm).HasColumnType("numeric(10,2)");
            e.Property(m => m.BicepsCm).HasColumnType("numeric(10,2)");
            e.Property(m => m.ThighCm).HasColumnType("numeric(10,2)");
            e.HasIndex(m => new { m.UserId, m.MeasuredAt });
        });

        builder.Entity<ExerciseLog>(e =>
        {
            e.Property(l => l.WeightKg).HasColumnType("numeric(10,2)");
            e.HasIndex(l => new { l.UserId, l.ExerciseId, l.PerformedAt });
        });
    }
}
