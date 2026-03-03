using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SessionService.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SessionDbContext>
{
    public SessionDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<SessionDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5437;Database=SessionDB;Username=reploop;Password=reploop123");
        return new SessionDbContext(optionsBuilder.Options);
    }
}
