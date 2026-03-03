using MediatR;
using UserService.Application.Features.Users.Common;
using UserService.Domain.Enums;

namespace UserService.Application.Features.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(
    string DisplayName,
    int? Age,
    int? HeightCm,
    decimal? WeightKg,
    ExperienceLevel? ExperienceLevel,
    FitnessGoal? Goal
) : IRequest<UserProfileDto>;
