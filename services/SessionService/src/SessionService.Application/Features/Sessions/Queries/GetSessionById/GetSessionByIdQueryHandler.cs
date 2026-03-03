using MediatR;
using Microsoft.EntityFrameworkCore;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionById;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, SessionDto?>
{
    private readonly ISessionDbContext _context;

    public GetSessionByIdQueryHandler(ISessionDbContext context)
    {
        _context = context;
    }

    public async Task<SessionDto?> Handle(GetSessionByIdQuery request, CancellationToken ct)
    {
        var session = await _context.Sessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId, ct);

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
