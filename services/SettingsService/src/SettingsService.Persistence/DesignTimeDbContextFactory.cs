using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SettingsService.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SettingsDbContext>
{
    public SettingsDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SettingsDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5436;Database=SettingsDB;Username=reploop;Password=reploop123");
        return new SettingsDbContext(optionsBuilder.Options);
    }
}
