using MediatR;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public DeleteWorkoutCommandHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(DeleteWorkoutCommand request, CancellationToken ct)
        => _manager.DeleteWorkoutAsync(request.Id, _currentUser.UserId, ct);
}
