using MediatR;
using WorkoutService.Application.Features.Workouts.Common;

namespace WorkoutService.Application.Features.Workouts.Queries.GetWorkoutById;

public record GetWorkoutByIdQuery(Guid Id, Guid UserId) : IRequest<WorkoutDto?>;
