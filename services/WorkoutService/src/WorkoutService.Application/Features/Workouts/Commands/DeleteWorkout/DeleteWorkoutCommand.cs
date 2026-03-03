using MediatR;

namespace WorkoutService.Application.Features.Workouts.Commands.DeleteWorkout;

public record DeleteWorkoutCommand(Guid Id, Guid UserId) : IRequest;
