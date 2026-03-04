using MediatR;
using StatisticsService.Application.Common.Interfaces;

namespace StatisticsService.Application.Features.Statistics.Commands.AddBodyMeasurement;

public class AddBodyMeasurementCommandHandler : IRequestHandler<AddBodyMeasurementCommand, Guid>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public AddBodyMeasurementCommandHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(AddBodyMeasurementCommand request, CancellationToken ct)
        => _manager.AddBodyMeasurementAsync(request, _currentUser.UserId, ct);
}
