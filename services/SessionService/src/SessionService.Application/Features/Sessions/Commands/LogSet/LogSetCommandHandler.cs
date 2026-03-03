using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Entities;
using SessionService.Domain.Enums;

namespace SessionService.Application.Features.Sessions.Commands.LogSet;

public class LogSetCommandHandler : IRequestHandler<LogSetCommand, Guid>
{
    private readonly ISessionDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public LogSetCommandHandler(ISessionDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(LogSetCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == request.SessionId && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", request.SessionId);

        if (session.Status != SessionStatus.Active && session.Status != SessionStatus.Paused)
            throw new ConflictException(ErrorCodes.SetLogNotAllowed, "Set yalnızca aktif veya duraklatılmış oturumlara eklenebilir.");

        var set = new SessionSet
        {
            SessionId = session.Id,
            ExerciseId = request.ExerciseId,
            ExerciseName = request.ExerciseName,
            SetNumber = request.SetNumber,
            Reps = request.Reps,
            WeightKg = request.WeightKg,
            DurationSeconds = request.DurationSeconds,
            Notes = request.Notes
        };

        _context.Sets.Add(set);
        await _context.SaveChangesAsync(ct);
        return set.Id;
    }
}
