using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutById;

public class GetWorkoutByIdQueryHandler : IRequestHandler<GetWorkoutByIdQuery, WorkoutDto?>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutByIdQueryHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<WorkoutDto?> Handle(GetWorkoutByIdQuery request, CancellationToken ct)
        => _manager.GetWorkoutByIdAsync(request.Id, _currentUser.UserId, ct);
}
