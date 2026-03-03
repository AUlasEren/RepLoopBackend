using MediatR;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateWorkoutCommandHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(UpdateWorkoutCommand request, CancellationToken ct)
        => _manager.UpdateWorkoutAsync(request, _currentUser.UserId, ct);
}
