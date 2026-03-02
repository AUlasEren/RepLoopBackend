using ExerciseService.Domain.Common;

namespace ExerciseService.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MuscleGroup { get; set; }
    public string? Equipment { get; set; }
    public string? Difficulty { get; set; }
    public string? VideoUrl { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsPublic { get; set; } = true;
    public Guid? CreatedByUserId { get; set; }
}
