using MediatR;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetPersonalRecords;

public class GetPersonalRecordsQueryHandler : IRequestHandler<GetPersonalRecordsQuery, List<PersonalRecordDto>>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetPersonalRecordsQueryHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<List<PersonalRecordDto>> Handle(GetPersonalRecordsQuery request, CancellationToken ct)
        => _manager.GetPersonalRecordsAsync(_currentUser.UserId, ct);
}
