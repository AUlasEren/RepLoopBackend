using SettingsService.Domain.Enums;

namespace SettingsService.Application.Features.Settings.Common;

public record WorkoutSettingsDto(
    WeightUnit WeightUnit,
    DistanceUnit DistanceUnit,
    int DefaultDurationMinutes,
    int RestBetweenSetsSeconds,
    List<string> WorkoutDays
);

public record NotificationSettingsDto(
    bool EmailNotifications,
    bool PushNotifications,
    bool WorkoutReminders,
    bool WeeklyReport,
    bool AchievementAlerts
);

public record PrivacySettingsDto(
    bool AllowDataAnalysis
);

public record SettingsDto(
    WorkoutSettingsDto Workout,
    NotificationSettingsDto Notifications,
    PrivacySettingsDto Privacy
);
