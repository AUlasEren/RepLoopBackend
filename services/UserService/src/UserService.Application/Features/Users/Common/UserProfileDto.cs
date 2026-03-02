using UserService.Domain.Enums;

namespace UserService.Application.Features.Users.Common;

public record UserProfileDto(
    Guid UserId,
    string DisplayName,
    int? Age,
    int? HeightCm,
    decimal? WeightKg,
    ExperienceLevel? ExperienceLevel,
    FitnessGoal? Goal,
    string? AvatarUrl
);
