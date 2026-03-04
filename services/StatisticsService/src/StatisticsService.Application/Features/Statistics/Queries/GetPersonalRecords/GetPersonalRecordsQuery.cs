using MediatR;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetPersonalRecords;

public record GetPersonalRecordsQuery : IRequest<List<PersonalRecordDto>>;
