using SessionService.Domain.Entities;

namespace SessionService.Application.Features.Sessions.Common;

public static class SessionMapper
{
    public static SessionDto ToDto(this WorkoutSession s) => new()
    {
        Id = s.Id,
        WorkoutId = s.WorkoutId,
        WorkoutName = s.WorkoutName,
        Status = s.Status.ToString(),
        StartedAt = s.StartedAt,
        PausedAt = s.PausedAt,
        CompletedAt = s.CompletedAt,
        TotalDurationSeconds = s.TotalDurationSeconds,
        Notes = s.Notes,
        Sets = s.Sets.OrderBy(set => set.CompletedAt).Select(set => set.ToDto()).ToList()
    };

    public static SessionSetDto ToDto(this SessionSet s) => new()
    {
        Id = s.Id,
        ExerciseId = s.ExerciseId,
        ExerciseName = s.ExerciseName,
        SetNumber = s.SetNumber,
        Reps = s.Reps,
        WeightKg = s.WeightKg,
        DurationSeconds = s.DurationSeconds,
        Notes = s.Notes,
        CompletedAt = s.CompletedAt
    };
}
