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
        var w = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == request.UserId, ct);

        if (w is null) return null;

        return new WorkoutDto
        {
            Id = w.Id,
            Name = w.Name,
            Description = w.Description,
            Notes = w.Notes,
            ScheduledDate = w.ScheduledDate,
            DurationMinutes = w.DurationMinutes,
            CreatedAt = w.CreatedAt,
            Exercises = w.WorkoutExercises.Select(e => new WorkoutExerciseDto
            {
                Id = e.Id,
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                WeightKg = e.WeightKg,
                DurationSeconds = e.DurationSeconds,
                Notes = e.Notes
            }).ToList()
        };
    }
}
