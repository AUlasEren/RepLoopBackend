using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using WorkoutService.Application.Common.Interfaces;
using WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;
using WorkoutService.Application.Features.Workouts.Commands.DuplicateWorkout;
using WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;
using WorkoutService.Application.Features.Workouts.Common;
using WorkoutService.Domain.Entities;

namespace WorkoutService.Application.Features.Workouts;

public class WorkoutsManager
{
    private readonly IWorkoutDbContext _context;

    public WorkoutsManager(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateWorkoutAsync(CreateWorkoutCommand command, Guid userId, CancellationToken ct)
    {
        var workout = new Workout
        {
            Name = command.Name,
            Description = command.Description,
            Notes = command.Notes,
            ScheduledDate = command.ScheduledDate,
            DurationMinutes = command.DurationMinutes,
            UserId = userId,
            WorkoutExercises = command.Exercises.Select(e => new WorkoutExercise
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                WeightKg = e.WeightKg,
                DurationSeconds = e.DurationSeconds,
                Notes = e.Notes
            }).ToList()
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(ct);
        return workout.Id;
    }

    public async Task UpdateWorkoutAsync(UpdateWorkoutCommand command, Guid userId, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == command.Id && w.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.WorkoutNotFound, "Workout", command.Id);

        workout.Name = command.Name;
        workout.Description = command.Description;
        workout.Notes = command.Notes;
        workout.ScheduledDate = command.ScheduledDate;
        workout.DurationMinutes = command.DurationMinutes;
        workout.UpdatedAt = DateTime.UtcNow;

        _context.WorkoutExercises.RemoveRange(workout.WorkoutExercises);
        workout.WorkoutExercises = command.Exercises.Select(e => new WorkoutExercise
        {
            ExerciseId = e.ExerciseId,
            ExerciseName = e.ExerciseName,
            Sets = e.Sets,
            Reps = e.Reps,
            WeightKg = e.WeightKg,
            DurationSeconds = e.DurationSeconds,
            Notes = e.Notes
        }).ToList();

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteWorkoutAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.WorkoutNotFound, "Workout", id);

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<WorkoutListDto> GetWorkoutsAsync(Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Workouts
            .Include(w => w.WorkoutExercises)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt);

        var totalCount = await query.CountAsync(ct);

        var workouts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new WorkoutListDto
        {
            Items = workouts.Select(w => w.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<WorkoutDto?> GetWorkoutByIdAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId, ct);

        return workout?.ToDto();
    }

    public async Task<Guid> DuplicateWorkoutAsync(DuplicateWorkoutCommand command, Guid userId, CancellationToken ct)
    {
        var original = await _context.Workouts
            .Include(w => w.WorkoutExercises)
            .FirstOrDefaultAsync(w => w.Id == command.Id && w.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.WorkoutNotFound, "Workout", command.Id);

        var workout = new Workout
        {
            Name = command.Name ?? $"{original.Name} (Kopya)",
            Description = original.Description,
            Notes = original.Notes,
            DurationMinutes = original.DurationMinutes,
            UserId = userId,
            WorkoutExercises = original.WorkoutExercises.Select(e => new WorkoutExercise
            {
                ExerciseId = e.ExerciseId,
                ExerciseName = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                WeightKg = e.WeightKg,
                DurationSeconds = e.DurationSeconds,
                Notes = e.Notes
            }).ToList()
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(ct);
        return workout.Id;
    }

    public async Task<WorkoutHistoryDto> GetWorkoutHistoryAsync(Guid userId, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Workouts
            .Include(w => w.WorkoutExercises)
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.CreatedAt);

        var totalCount = await query.CountAsync(ct);

        var workouts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new WorkoutHistoryDto
        {
            Items = workouts.Select(w => w.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
