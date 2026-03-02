using MediatR;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExerciseById;

public record GetExerciseByIdQuery(Guid Id) : IRequest<ExerciseDto?>;
