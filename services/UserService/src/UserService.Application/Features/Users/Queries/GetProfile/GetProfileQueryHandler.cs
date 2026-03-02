using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Common;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUserDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetProfileQueryHandler(IUserDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
        {
            // İlk girişte profili otomatik oluştur
            profile = new UserProfile { UserId = userId };
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ToDto(profile);
    }

    private static UserProfileDto ToDto(UserProfile p) =>
        new(p.UserId, p.DisplayName, p.Age, p.HeightCm, p.WeightKg, p.ExperienceLevel, p.Goal, p.AvatarUrl);
}
