using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly IWorkoutDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkoutCommandHandler(IWorkoutDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var workout = new Workout
        {
            Name = request.Name,
            Description = request.Description,
            Notes = request.Notes,
            ScheduledDate = request.ScheduledDate,
            DurationMinutes = request.DurationMinutes,
            UserId = userId,
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
