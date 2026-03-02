using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Features.Users.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IUserDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorageService _fileStorage;

    public DeleteAccountCommandHandler(
        IUserDbContext context,
        ICurrentUserService currentUserService,
        IFileStorageService fileStorage)
    {
        _context = context;
        _currentUserService = currentUserService;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (profile is null)
            return;

        if (!string.IsNullOrEmpty(profile.AvatarUrl))
            _fileStorage.DeleteAvatar(profile.AvatarUrl);

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
