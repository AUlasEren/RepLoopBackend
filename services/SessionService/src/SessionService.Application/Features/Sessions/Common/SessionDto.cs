namespace SessionService.Application.Features.Sessions.Common;

public class SessionDto
{
    public Guid Id { get; set; }
    public Guid WorkoutId { get; set; }
    public string WorkoutName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? PausedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? TotalDurationSeconds { get; set; }
    public string? Notes { get; set; }
    public List<SessionSetDto> Sets { get; set; } = new();
}
