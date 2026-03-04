using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StatisticsService.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StatisticsDbContext>
{
    public StatisticsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StatisticsDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5438;Database=StatisticsDB;Username=reploop;Password=reploop123");
        return new StatisticsDbContext(optionsBuilder.Options);
    }
}
