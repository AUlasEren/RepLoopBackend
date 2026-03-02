namespace WorkoutService.Application.Features.Workouts.Common;

public class WorkoutDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public int? DurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<WorkoutExerciseDto> Exercises { get; set; } = new();
}
