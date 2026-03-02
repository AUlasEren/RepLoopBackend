using Microsoft.EntityFrameworkCore;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Persistence;

public class WorkoutDbContext : DbContext, IWorkoutDbContext
{
    public WorkoutDbContext(DbContextOptions<WorkoutDbContext> options) : base(options) { }

    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutExercise> WorkoutExercises => Set<WorkoutExercise>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<WorkoutExercise>()
            .HasOne(we => we.Workout)
            .WithMany(w => w.WorkoutExercises)
            .HasForeignKey(we => we.WorkoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WorkoutExercise>()
            .Property(we => we.WeightKg)
            .HasColumnType("numeric(10,2)");
    }
}
