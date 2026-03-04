using MediatR;
using StatisticsService.Application.Common.Interfaces;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Commands.UpdateBodyMeasurement;

public class UpdateBodyMeasurementCommandHandler : IRequestHandler<UpdateBodyMeasurementCommand, BodyMeasurementDto>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateBodyMeasurementCommandHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<BodyMeasurementDto> Handle(UpdateBodyMeasurementCommand request, CancellationToken ct)
        => _manager.UpdateBodyMeasurementAsync(request, _currentUser.UserId, ct);
}
