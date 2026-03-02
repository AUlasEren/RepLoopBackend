using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Persistence;

public class UserDbContext : DbContext, IUserDbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(p => p.UserId);

            entity.Property(p => p.DisplayName)
                .HasMaxLength(100)
                .HasDefaultValue(string.Empty);

            entity.Property(p => p.WeightKg)
                .HasColumnType("numeric(6,2)");

            entity.Property(p => p.ExperienceLevel)
                .HasConversion<string>();

            entity.Property(p => p.Goal)
                .HasConversion<string>();
        });
    }
}
