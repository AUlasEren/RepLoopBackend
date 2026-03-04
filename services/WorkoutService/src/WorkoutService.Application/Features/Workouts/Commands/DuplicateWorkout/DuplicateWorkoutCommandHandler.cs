using MediatR;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.DuplicateWorkout;

public class DuplicateWorkoutCommandHandler : IRequestHandler<DuplicateWorkoutCommand, Guid>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public DuplicateWorkoutCommandHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(DuplicateWorkoutCommand request, CancellationToken ct)
        => _manager.DuplicateWorkoutAsync(request, _currentUser.UserId, ct);
}
