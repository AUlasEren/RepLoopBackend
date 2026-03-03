using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Common;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUserDbContext _context;

    public GetProfileQueryHandler(IUserDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
        {
            profile = new UserProfile { UserId = request.UserId };
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ToDto(profile);
    }

    private static UserProfileDto ToDto(UserProfile p) =>
        new(p.UserId, p.DisplayName, p.Age, p.HeightCm, p.WeightKg, p.ExperienceLevel, p.Goal, p.AvatarUrl);
}
