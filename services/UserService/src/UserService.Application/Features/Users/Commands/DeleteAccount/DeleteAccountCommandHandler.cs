using MediatR;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.Features.Users.Commands.DeleteAccount;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand>
{
    private readonly IUserDbContext _context;
    private readonly IFileStorageService _fileStorage;

    public DeleteAccountCommandHandler(IUserDbContext context, IFileStorageService fileStorage)
    {
        _context = context;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == request.UserId, cancellationToken);

        if (profile is null)
            return;

        if (!string.IsNullOrEmpty(profile.AvatarUrl))
            _fileStorage.DeleteAvatar(profile.AvatarUrl);

        _context.UserProfiles.Remove(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
