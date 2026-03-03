using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutHistory;

public record GetWorkoutHistoryQuery(int Page = 1, int PageSize = 10) : IRequest<WorkoutHistoryDto>;
