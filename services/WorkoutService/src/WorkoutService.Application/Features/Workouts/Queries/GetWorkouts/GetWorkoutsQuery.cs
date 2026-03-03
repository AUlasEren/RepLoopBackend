using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

public record GetWorkoutsQuery(Guid UserId) : IRequest<List<WorkoutDto>>;
