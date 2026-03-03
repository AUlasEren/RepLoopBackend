using Microsoft.EntityFrameworkCore;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Entities;

namespace SessionService.Persistence;

public class SessionDbContext : DbContext, ISessionDbContext
{
    public SessionDbContext(DbContextOptions<SessionDbContext> options) : base(options) { }

    public DbSet<WorkoutSession> Sessions => Set<WorkoutSession>();
    public DbSet<SessionSet> Sets => Set<SessionSet>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<SessionSet>()
            .HasOne(s => s.Session)
            .WithMany(ws => ws.Sets)
            .HasForeignKey(s => s.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<SessionSet>()
            .Property(s => s.WeightKg)
            .HasColumnType("numeric(10,2)");

        builder.Entity<WorkoutSession>()
            .Property(s => s.Status)
            .HasConversion<string>();
    }
}
