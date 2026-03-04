using MediatR;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetBodyMeasurements;

public class GetBodyMeasurementsQueryHandler : IRequestHandler<GetBodyMeasurementsQuery, PaginatedResult<BodyMeasurementDto>>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetBodyMeasurementsQueryHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<PaginatedResult<BodyMeasurementDto>> Handle(GetBodyMeasurementsQuery request, CancellationToken ct)
        => _manager.GetBodyMeasurementsAsync(_currentUser.UserId, request.Page, request.PageSize, ct);
}
