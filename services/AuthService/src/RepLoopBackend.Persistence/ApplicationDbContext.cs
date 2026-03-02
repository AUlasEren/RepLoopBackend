using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Domain.Entities;
using RepLoopBackend.Persistence.Entities;

namespace RepLoopBackend.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Workout>()
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Workouts)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();
    }
}
