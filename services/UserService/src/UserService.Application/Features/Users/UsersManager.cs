using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Application.Features.Users.Commands.UpdateAvatar;
using UserService.Application.Features.Users.Commands.UpdateProfile;
using UserService.Application.Features.Users.Common;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users;

public class UsersManager
{
    private readonly IUserDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public UsersManager(IUserDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            profile = new UserProfile { UserId = userId };
            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync(ct);
        }

        return ToDto(profile);
    }

    public async Task<UserProfileDto> UpdateProfileAsync(UpdateProfileCommand command, Guid userId, CancellationToken ct)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            profile = new UserProfile { UserId = userId };
            _context.UserProfiles.Add(profile);
        }

        profile.DisplayName = command.DisplayName;
        profile.Age = command.Age;
        profile.HeightCm = command.HeightCm;
        profile.WeightKg = command.WeightKg;
        profile.ExperienceLevel = command.ExperienceLevel;
        profile.Goal = command.Goal;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return ToDto(profile);
    }

    public async Task<string> UpdateAvatarAsync(UpdateAvatarCommand command, Guid userId, CancellationToken ct)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
        {
            profile = new UserProfile { UserId = userId };
            _context.UserProfiles.Add(profile);
        }

        if (!string.IsNullOrEmpty(profile.AvatarUrl))
            _fileStorage.DeleteAvatar(profile.AvatarUrl);

        var avatarUrl = await _fileStorage.SaveAvatarAsync(
            userId, command.FileStream, command.ContentType, ct);

        profile.AvatarUrl = avatarUrl;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
        return avatarUrl;
    }

    public async Task DeleteAccountAsync(Guid userId, CancellationToken ct)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is null)
            return;

        if (!string.IsNullOrEmpty(profile.AvatarUrl))
            _fileStorage.DeleteAvatar(profile.AvatarUrl);

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync(ct);
    }

    private static UserProfileDto ToDto(UserProfile p) =>
        new(p.UserId, p.DisplayName, p.Age, p.HeightCm, p.WeightKg, p.ExperienceLevel, p.Goal, p.AvatarUrl);
}
