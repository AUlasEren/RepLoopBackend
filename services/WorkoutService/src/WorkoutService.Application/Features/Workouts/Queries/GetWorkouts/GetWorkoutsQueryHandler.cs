using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

public class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, WorkoutListDto>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutsQueryHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<WorkoutListDto> Handle(GetWorkoutsQuery request, CancellationToken ct)
        => _manager.GetWorkoutsAsync(_currentUser.UserId, request.Page, request.PageSize, ct);
}
