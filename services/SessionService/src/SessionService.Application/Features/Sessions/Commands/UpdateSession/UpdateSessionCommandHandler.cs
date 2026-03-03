using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Enums;

namespace SessionService.Application.Features.Sessions.Commands.UpdateSession;

public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand>
{
    private readonly ISessionDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateSessionCommandHandler(ISessionDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateSessionCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", request.Id);

        switch (request.Action)
        {
            case SessionAction.Pause:
                if (session.Status != SessionStatus.Active)
                    throw new ConflictException(ErrorCodes.PauseNotAllowed, "Yalnızca aktif oturumlar duraklatılabilir.");
                session.Status = SessionStatus.Paused;
                session.PausedAt = DateTime.UtcNow;
                break;

            case SessionAction.Resume:
                if (session.Status != SessionStatus.Paused)
                    throw new ConflictException(ErrorCodes.ResumeNotAllowed, "Yalnızca duraklatılmış oturumlar devam ettirilebilir.");
                session.Status = SessionStatus.Active;
                session.PausedAt = null;
                break;
        }

        session.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
    }
}