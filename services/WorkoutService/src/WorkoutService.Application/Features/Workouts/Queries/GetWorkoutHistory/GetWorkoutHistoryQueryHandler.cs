using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutHistory;

public class GetWorkoutHistoryQueryHandler : IRequestHandler<GetWorkoutHistoryQuery, WorkoutHistoryDto>
{
    private readonly IWorkoutDbContext _context;

    public GetWorkoutHistoryQueryHandler(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutHistoryDto> Handle(GetWorkoutHistoryQuery request, CancellationToken ct)
    {
        var query = _context.Workouts
            .Include(w => w.WorkoutExercises)
            .Where(w => w.UserId == request.UserId)
            .OrderByDescending(w => w.CreatedAt);

        var totalCount = await query.CountAsync(ct);

        var workouts = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new WorkoutHistoryDto
        {
            Items = workouts.Select(w => w.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
