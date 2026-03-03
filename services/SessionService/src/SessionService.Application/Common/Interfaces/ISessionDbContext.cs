using Microsoft.EntityFrameworkCore;
using SessionService.Domain.Entities;

namespace SessionService.Application.Common.Interfaces;

public interface ISessionDbContext
{
    DbSet<WorkoutSession> Sessions { get; }
    DbSet<SessionSet> Sets { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
