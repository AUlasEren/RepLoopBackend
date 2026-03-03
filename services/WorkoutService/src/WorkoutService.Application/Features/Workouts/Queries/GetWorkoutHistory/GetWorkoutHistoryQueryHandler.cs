using MediatR;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutHistory;

public class GetWorkoutHistoryQueryHandler : IRequestHandler<GetWorkoutHistoryQuery, WorkoutHistoryDto>
{
    private readonly WorkoutsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetWorkoutHistoryQueryHandler(WorkoutsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<WorkoutHistoryDto> Handle(GetWorkoutHistoryQuery request, CancellationToken ct)
        => _manager.GetWorkoutHistoryAsync(_currentUser.UserId, request.Page, request.PageSize, ct);
}
