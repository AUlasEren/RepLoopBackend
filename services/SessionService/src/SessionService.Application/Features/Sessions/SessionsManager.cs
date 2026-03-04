using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Commands.AbandonSession;
using SessionService.Application.Features.Sessions.Commands.CompleteSession;
using SessionService.Application.Features.Sessions.Commands.LogSet;
using SessionService.Application.Features.Sessions.Commands.StartSession;
using SessionService.Application.Features.Sessions.Commands.UpdateSession;
using SessionService.Application.Features.Sessions.Common;
using SessionService.Domain.Entities;
using SessionService.Domain.Enums;

namespace SessionService.Application.Features.Sessions;

public class SessionsManager
{
    private readonly ISessionDbContext _context;

    public SessionsManager(ISessionDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> StartSessionAsync(StartSessionCommand command, Guid userId, CancellationToken ct)
    {
        var session = new WorkoutSession
        {
            UserId = userId,
            WorkoutId = command.WorkoutId,
            WorkoutName = command.WorkoutName
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(ct);
        return session.Id;
    }

    public async Task<Guid> LogSetAsync(LogSetCommand command, Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.SessionId && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", command.SessionId);

        if (session.Status != SessionStatus.Active && session.Status != SessionStatus.Paused)
            throw new ConflictException(ErrorCodes.SetLogNotAllowed, "Set yalnızca aktif veya duraklatılmış oturumlara eklenebilir.");

        var set = new SessionSet
        {
            SessionId = session.Id,
            ExerciseId = command.ExerciseId,
            ExerciseName = command.ExerciseName,
            SetNumber = command.SetNumber,
            Reps = command.Reps,
            WeightKg = command.WeightKg,
            DurationSeconds = command.DurationSeconds,
            Notes = command.Notes
        };

        _context.Sets.Add(set);
        await _context.SaveChangesAsync(ct);
        return set.Id;
    }

    public async Task UpdateSessionAsync(UpdateSessionCommand command, Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.Id && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", command.Id);

        switch (command.Action)
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

    public async Task CompleteSessionAsync(CompleteSessionCommand command, Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.Id && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", command.Id);

        if (session.Status == SessionStatus.Completed)
            throw new ConflictException(ErrorCodes.SessionAlreadyCompleted, "Bu antrenman oturumu zaten tamamlanmış.");

        var completedAt = DateTime.UtcNow;
        session.Status = SessionStatus.Completed;
        session.CompletedAt = completedAt;
        session.TotalDurationSeconds = (int)(completedAt - session.StartedAt).TotalSeconds;
        session.Notes = command.Notes ?? session.Notes;
        session.UpdatedAt = completedAt;

        await _context.SaveChangesAsync(ct);
    }

    public async Task<SessionDto?> GetActiveSessionAsync(Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.UserId == userId
                && (s.Status == SessionStatus.Active || s.Status == SessionStatus.Paused), ct);

        return session?.ToDto();
    }

    public async Task AbandonSessionAsync(AbandonSessionCommand command, Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == command.Id && s.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.SessionNotFound, "Session", command.Id);

        if (session.Status == SessionStatus.Completed || session.Status == SessionStatus.Abandoned)
            throw new ConflictException(ErrorCodes.SessionAlreadyFinished, "Bu antrenman oturumu zaten tamamlanmış veya terk edilmiş.");

        var now = DateTime.UtcNow;
        session.Status = SessionStatus.Abandoned;
        session.CompletedAt = now;
        session.TotalDurationSeconds = (int)(now - session.StartedAt).TotalSeconds;
        session.Notes = command.Notes ?? session.Notes;
        session.UpdatedAt = now;

        await _context.SaveChangesAsync(ct);
    }

    public async Task<SessionDto?> GetSessionByIdAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var session = await _context.Sessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId, ct);

        return session?.ToDto();
    }

    public async Task<SessionHistoryDto> GetSessionHistoryAsync(Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Sessions
            .Include(s => s.Sets)
            .Where(s => s.UserId == userId && s.Status == SessionStatus.Completed)
            .OrderByDescending(s => s.CompletedAt);

        var totalCount = await query.CountAsync(ct);

        var sessions = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new SessionHistoryDto
        {
            Items = sessions.Select(s => s.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
