using MediatR;

namespace ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;

public record UpdateExerciseCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? MuscleGroup { get; init; }
    public string? Equipment { get; init; }
    public string? Difficulty { get; init; }
    public string? VideoUrl { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsPublic { get; init; } = true;
}
