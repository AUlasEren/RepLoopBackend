using MediatR;
using StatisticsService.Application.Common.Interfaces;

namespace StatisticsService.Application.Features.Statistics.Commands.DeleteBodyMeasurement;

public class DeleteBodyMeasurementCommandHandler : IRequestHandler<DeleteBodyMeasurementCommand>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public DeleteBodyMeasurementCommandHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(DeleteBodyMeasurementCommand request, CancellationToken ct)
        => _manager.DeleteBodyMeasurementAsync(request.Id, _currentUser.UserId, ct);
}
