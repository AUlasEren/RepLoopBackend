using MediatR;

namespace ExerciseService.Application.Features.Exercises.Commands.DeleteExercise;

public record DeleteExerciseCommand(Guid Id) : IRequest;
