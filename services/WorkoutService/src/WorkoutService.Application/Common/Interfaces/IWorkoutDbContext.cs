using Microsoft.EntityFrameworkCore;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Application.Common.Interfaces;

public interface IWorkoutDbContext
{
    DbSet<Workout> Workouts { get; }
    DbSet<WorkoutExercise> WorkoutExercises { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
