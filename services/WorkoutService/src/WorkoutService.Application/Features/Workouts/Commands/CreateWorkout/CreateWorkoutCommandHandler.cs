using MediatR;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public CreateWorkoutCommandHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken ct)
        => _manager.CreateWorkoutAsync(request, _currentUser.UserId, ct);
}
