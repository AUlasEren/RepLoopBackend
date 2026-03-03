using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

public class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, List<WorkoutDto>>
{
    private readonly IWorkoutDbContext _context;

    public GetWorkoutsQueryHandler(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<List<WorkoutDto>> Handle(GetWorkoutsQuery request, CancellationToken ct)
    {
        var workouts = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .Where(w => w.UserId == request.UserId)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(ct);

        return workouts.Select(w => new WorkoutDto
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
        }).ToList();
    }
}
