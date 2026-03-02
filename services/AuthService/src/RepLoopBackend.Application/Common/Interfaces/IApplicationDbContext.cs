using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Domain.Entities;

namespace RepLoopBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workout> Workouts { get; }
    DbSet<Exercise> Exercises { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
