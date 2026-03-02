using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ExerciseService.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ExerciseDbContext>
{
    public ExerciseDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExerciseDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5434;Database=ExerciseDB;Username=reploop;Password=reploop123");
        return new ExerciseDbContext(optionsBuilder.Options);
    }
}
