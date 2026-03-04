using Microsoft.EntityFrameworkCore;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Domain.Entities;

namespace ExerciseService.Persistence;

public class ExerciseDbContext : DbContext, IExerciseDbContext
{
    public ExerciseDbContext(DbContextOptions<ExerciseDbContext> options) : base(options) { }

    public DbSet<Exercise> Exercises => Set<Exercise>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExerciseDbContext).Assembly);
    }
}
