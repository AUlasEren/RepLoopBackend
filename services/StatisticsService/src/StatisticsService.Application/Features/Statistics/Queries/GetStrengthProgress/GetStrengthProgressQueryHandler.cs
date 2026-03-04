using MediatR;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetStrengthProgress;

public class GetStrengthProgressQueryHandler : IRequestHandler<GetStrengthProgressQuery, StrengthProgressDto>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetStrengthProgressQueryHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<StrengthProgressDto> Handle(GetStrengthProgressQuery request, CancellationToken ct)
        => _manager.GetStrengthProgressAsync(_currentUser.UserId, request.ExerciseId, request.Period, ct);
}
