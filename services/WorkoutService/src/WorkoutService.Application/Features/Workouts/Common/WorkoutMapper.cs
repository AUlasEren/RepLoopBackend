using WorkoutService.Domain.Entities;

namespace WorkoutService.Application.Features.Workouts.Common;

public static class WorkoutMapper
{
    public static WorkoutDto ToDto(this Workout w) => new()
    {
        Id = w.Id,
        Name = w.Name,
        Description = w.Description,
        Notes = w.Notes,
        ScheduledDate = w.ScheduledDate,
        DurationMinutes = w.DurationMinutes,
        CreatedAt = w.CreatedAt,
        Exercises = w.WorkoutExercises.Select(e => e.ToDto()).ToList()
    };

    public static WorkoutExerciseDto ToDto(this WorkoutExercise e) => new()
    {
        Id = e.Id,
        ExerciseId = e.ExerciseId,
        ExerciseName = e.ExerciseName,
        Sets = e.Sets,
        Reps = e.Reps,
        WeightKg = e.WeightKg,
        DurationSeconds = e.DurationSeconds,
        Notes = e.Notes
    };
}
