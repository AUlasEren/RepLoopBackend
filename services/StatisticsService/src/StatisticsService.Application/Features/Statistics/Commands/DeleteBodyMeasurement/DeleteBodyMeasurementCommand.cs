using MediatR;

namespace StatisticsService.Application.Features.Statistics.Commands.DeleteBodyMeasurement;

public record DeleteBodyMeasurementCommand(Guid Id) : IRequest;
