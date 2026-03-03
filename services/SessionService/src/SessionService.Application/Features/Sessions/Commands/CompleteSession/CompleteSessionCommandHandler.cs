using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Enums;

namespace SessionService.Application.Features.Sessions.Commands.CompleteSession;

public class CompleteSessionCommandHandler : IRequestHandler<CompleteSessionCommand>
{
    private readonly ISessionDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CompleteSessionCommandHandler(ISessionDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(CompleteSessionCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", request.Id);

        if (session.Status == SessionStatus.Completed)
            throw new ConflictException(ErrorCodes.SessionAlreadyCompleted, "Bu antrenman oturumu zaten tamamlanmış.");

        var completedAt = DateTime.UtcNow;
        session.Status = SessionStatus.Completed;
        session.CompletedAt = completedAt;
        session.TotalDurationSeconds = (int)(completedAt - session.StartedAt).TotalSeconds;
        session.Notes = request.Notes ?? session.Notes;
        session.UpdatedAt = completedAt;

        await _context.SaveChangesAsync(ct);
    }
}
