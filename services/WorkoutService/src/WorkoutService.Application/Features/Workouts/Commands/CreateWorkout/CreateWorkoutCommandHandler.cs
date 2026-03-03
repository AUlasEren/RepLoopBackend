using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly IWorkoutDbContext _context;

    public CreateWorkoutCommandHandler(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken ct)
    {
        var workout = new Workout
        {
            Name = request.Name,
            Description = request.Description,
            Notes = request.Notes,
            ScheduledDate = request.ScheduledDate,
            DurationMinutes = request.DurationMinutes,
            UserId = request.UserId,
            WorkoutExercises = request.Exercises.Select(e => new WorkoutExercise
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                WeightKg = e.WeightKg,
                DurationSeconds = e.DurationSeconds,
                Notes = e.Notes
            }).ToList()
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(ct);
        return workout.Id;
    }
}
