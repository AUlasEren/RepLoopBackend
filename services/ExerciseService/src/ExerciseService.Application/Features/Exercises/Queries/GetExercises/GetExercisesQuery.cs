using MediatR;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExercises;

public record GetExercisesQuery(string? MuscleGroup = null, string? Equipment = null, string? Difficulty = null) : IRequest<List<ExerciseDto>>;
