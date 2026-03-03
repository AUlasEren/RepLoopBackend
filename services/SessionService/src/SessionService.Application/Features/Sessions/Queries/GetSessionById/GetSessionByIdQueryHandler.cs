using MediatR;
using Microsoft.EntityFrameworkCore;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionById;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, SessionDto?>
{
    private readonly ISessionDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSessionByIdQueryHandler(ISessionDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<SessionDto?> Handle(GetSessionByIdQuery request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = await _context.Sessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == userId, ct);

        if (session is null) return null;

        return new SessionDto
        {
            Id = session.Id,
            WorkoutId = session.WorkoutId,
            WorkoutName = session.WorkoutName,
            Status = session.Status.ToString(),
            StartedAt = session.StartedAt,
            PausedAt = session.PausedAt,
            CompletedAt = session.CompletedAt,
            TotalDurationSeconds = session.TotalDurationSeconds,
            Notes = session.Notes,
            Sets = session.Sets.OrderBy(s => s.CompletedAt).Select(s => new SessionSetDto
            {
                Id = s.Id,
                ExerciseId = s.ExerciseId,
                ExerciseName = s.ExerciseName,
                SetNumber = s.SetNumber,
                Reps = s.Reps,
                WeightKg = s.WeightKg,
                DurationSeconds = s.DurationSeconds,
                Notes = s.Notes,
                CompletedAt = s.CompletedAt
            }).ToList()
        };
    }
}
