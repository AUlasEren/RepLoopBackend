using MediatR;
using StatisticsService.Application.Features.Statistics.Common;

namespace StatisticsService.Application.Features.Statistics.Queries.GetStrengthProgress;

public record GetStrengthProgressQuery(Guid ExerciseId, string Period = "30d") : IRequest<StrengthProgressDto>;
