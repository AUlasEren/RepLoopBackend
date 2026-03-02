using WorkoutService.Domain.Common;

namespace WorkoutService.Domain.Entities;

public class Workout : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public int? DurationMinutes { get; set; }
    public Guid UserId { get; set; }
    public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
}
