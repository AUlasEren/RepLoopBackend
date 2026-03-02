using RepLoopBackend.Domain.Common;

namespace RepLoopBackend.Domain.Entities;

public class Exercise : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? MuscleGroup { get; set; }

    public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
}
