using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutById;

public class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly IWorkoutDbContext _context;

    public GetWorkoutByIdQueryHandler(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<WorkoutDto?> Handle(GetWorkoutByIdQuery request, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == request.UserId, ct);

        return workout?.ToDto();
    }
}
