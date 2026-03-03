using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.UpdateAvatar;

public class UpdateAvatarCommandHandler : IRequestHandler<UpdateAvatarCommand, string>
{
    private readonly IUserDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public UpdateAvatarCommandHandler(IUserDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task<string> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
        {
            profile = new UserProfile { UserId = request.UserId };
            _context.UserProfiles.Add(profile);
        }

        if (!string.IsNullOrEmpty(profile.AvatarUrl))
            _fileStorage.DeleteAvatar(profile.AvatarUrl);

        var avatarUrl = await _fileStorage.SaveAvatarAsync(
            request.UserId, request.FileStream, request.ContentType, cancellationToken);

        profile.AvatarUrl = avatarUrl;
        profile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return avatarUrl;
    }
}
