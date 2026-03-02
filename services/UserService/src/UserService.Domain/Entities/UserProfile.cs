using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public class UserProfile
{
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public int? Age { get; set; }
    public int? HeightCm { get; set; }
    public decimal? WeightKg { get; set; }
    public ExperienceLevel? ExperienceLevel { get; set; }
    public FitnessGoal? Goal { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
