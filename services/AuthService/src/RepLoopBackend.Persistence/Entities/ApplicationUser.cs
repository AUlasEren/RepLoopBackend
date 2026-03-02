using Microsoft.AspNetCore.Identity;
using RepLoopBackend.Domain.Entities;

namespace RepLoopBackend.Persistence.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public bool IsProfileComplete { get; set; }
    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
