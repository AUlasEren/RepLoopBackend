using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Common;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly IUserDbContext _context;

    public UpdateProfileCommandHandler(IUserDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
        {
            profile = new UserProfile { UserId = request.UserId };
            _context.UserProfiles.Add(profile);
        }

        profile.DisplayName = request.DisplayName;
        profile.Age = request.Age;
        profile.HeightCm = request.HeightCm;
        profile.WeightKg = request.WeightKg;
        profile.ExperienceLevel = request.ExperienceLevel;
        profile.Goal = request.Goal;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return new UserProfileDto(
            profile.UserId, profile.DisplayName, profile.Age,
            profile.HeightCm, profile.WeightKg, profile.ExperienceLevel,
            profile.Goal, profile.AvatarUrl);
    }
}
