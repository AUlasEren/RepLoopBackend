using Microsoft.EntityFrameworkCore;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Common.Interfaces;

public interface ISettingsDbContext
{
    DbSet<UserSettings> UserSettings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
