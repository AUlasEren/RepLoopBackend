using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

public record GetWorkoutsQuery(int Page = 1, int PageSize = 20) : IRequest<WorkoutListDto>;
