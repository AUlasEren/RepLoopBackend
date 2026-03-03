using SessionService.Domain.Common;
using SessionService.Domain.Enums;

namespace SessionService.Domain.Entities;

public class WorkoutSession : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid WorkoutId { get; set; }
    public string WorkoutName { get; set; } = string.Empty;
    public SessionStatus Status { get; set; } = SessionStatus.Active;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? PausedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? TotalDurationSeconds { get; set; }
    public string? Notes { get; set; }

    public ICollection<SessionSet> Sets { get; set; } = new List<SessionSet>();
}
