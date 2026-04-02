using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkoutFromTemplate;

public class CreateWorkoutFromTemplateCommandHandler
    : IRequestHandler<CreateWorkoutFromTemplateCommand, Guid>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public CreateWorkoutFromTemplateCommandHandler(
        WorkoutsManager manager,
        ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(CreateWorkoutFromTemplateCommand request, CancellationToken ct)
    {
        var createCommand = new CreateWorkoutCommand
        {
            Name = request.Name,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            Exercises = request.Exercises
                .OrderBy(e => e.Order)
                .Select(e => new WorkoutExerciseItem
                {
                    ExerciseId = e.ExerciseId,
                    ExerciseName = e.ExerciseName,
                    Sets = e.Sets,
                    Reps = e.Reps,
                })
                .ToList()
        };

        return _manager.CreateWorkoutAsync(createCommand, _currentUser.UserId, ct);
    }
}
