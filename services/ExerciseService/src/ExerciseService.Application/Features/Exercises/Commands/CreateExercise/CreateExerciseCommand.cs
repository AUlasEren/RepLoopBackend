using MediatR;

namespace ExerciseService.Application.Features.Exercises.Commands.CreateExercise;

public record CreateExerciseCommand : IRequest<Guid>
{
    public Guid? CreatedByUserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? MuscleGroup { get; init; }
    public string? Equipment { get; init; }
    public string? Difficulty { get; init; }
    public string? VideoUrl { get; init; }
    public string? ImageUrl { get; init; }
    public bool IsPublic { get; init; } = true;
}
