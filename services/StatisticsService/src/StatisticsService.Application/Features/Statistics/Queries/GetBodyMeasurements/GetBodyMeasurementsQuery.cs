using MediatR;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetBodyMeasurements;

public record GetBodyMeasurementsQuery(int Page = 1, int PageSize = 10) : IRequest<PaginatedResult<BodyMeasurementDto>>;
