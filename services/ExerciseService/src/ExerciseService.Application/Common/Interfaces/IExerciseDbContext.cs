using Microsoft.EntityFrameworkCore;
using ExerciseService.Domain.Entities;

namespace ExerciseService.Application.Common.Interfaces;

public interface IExerciseDbContext
{
    DbSet<Exercise> Exercises { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
