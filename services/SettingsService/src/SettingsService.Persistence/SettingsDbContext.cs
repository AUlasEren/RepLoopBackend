using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Domain.Entities;

namespace SettingsService.Persistence;

public class SettingsDbContext : DbContext, ISettingsDbContext
{
    public SettingsDbContext(DbContextOptions<SettingsDbContext> options) : base(options) { }

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserSettings>(entity =>
        {
            entity.HasKey(s => s.UserId);

            entity.Property(s => s.WorkoutDays)
                .HasMaxLength(200)
                .HasDefaultValue("Monday,Wednesday,Friday");

            entity.Property(s => s.WeightUnit)
                .HasConversion<string>();

            entity.Property(s => s.DistanceUnit)
                .HasConversion<string>();
        });
    }
}
