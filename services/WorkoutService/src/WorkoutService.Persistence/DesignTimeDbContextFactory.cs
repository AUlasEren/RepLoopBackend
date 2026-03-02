using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkoutService.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<WorkoutDbContext>
{
    public WorkoutDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkoutDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=WorkoutDB;Username=reploop;Password=reploop123");
        return new WorkoutDbContext(optionsBuilder.Options);
    }
}
