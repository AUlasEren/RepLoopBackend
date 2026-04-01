using MediatR;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkoutFromTemplate;

public record CreateWorkoutFromTemplateCommand : IRequest<Guid>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int DurationMinutes { get; init; }
    public string? Difficulty { get; init; }
    public List<TemplateExerciseDto> Exercises { get; init; } = new();
}

public record TemplateExerciseDto
{
    public Guid ExerciseId { get; init; }
    public string ExerciseName { get; init; } = string.Empty;
    public int Order { get; init; }
    public int Sets { get; init; }
    public int Reps { get; init; }
}
