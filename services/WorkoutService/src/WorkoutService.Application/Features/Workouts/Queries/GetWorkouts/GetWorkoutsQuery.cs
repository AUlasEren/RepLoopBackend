using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

public record GetWorkoutsQuery : IRequest<List<WorkoutDto>>;
