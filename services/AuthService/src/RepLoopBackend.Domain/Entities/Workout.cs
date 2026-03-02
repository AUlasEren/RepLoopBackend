using RepLoopBackend.Domain.Common;

namespace RepLoopBackend.Domain.Entities;

public class Workout : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid UserId { get; set; }

    public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
