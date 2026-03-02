namespace ExerciseService.Application.Features.Exercises.Common;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MuscleGroup { get; set; }
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
}
