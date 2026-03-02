using SettingsService.Domain.Enums;

namespace SettingsService.Domain.Entities;

public class UserSettings
{
    public Guid UserId { get; set; }

    // Workout preferences
    public WeightUnit WeightUnit { get; set; } = WeightUnit.Kg;
    public DistanceUnit DistanceUnit { get; set; } = DistanceUnit.Km;
    public int DefaultDurationMinutes { get; set; } = 60;
    public int RestBetweenSetsSeconds { get; set; } = 60;
    public string WorkoutDays { get; set; } = "Monday,Wednesday,Friday";

    // Notifications
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = true;
    public bool WorkoutReminders { get; set; } = true;
    public bool WeeklyReport { get; set; } = false;
    public bool AchievementAlerts { get; set; } = true;

    // Privacy
    public bool AllowDataAnalysis { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
