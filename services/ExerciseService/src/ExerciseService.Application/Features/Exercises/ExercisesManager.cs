using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Commands.CreateExercise;
using ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;
using ExerciseService.Application.Features.Exercises.Common;
using ExerciseService.Domain.Entities;

namespace ExerciseService.Application.Features.Exercises;

public class ExercisesManager
{
    private readonly IExerciseDbContext _context;

    public ExercisesManager(IExerciseDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> CreateExerciseAsync(CreateExerciseCommand command, Guid userId, CancellationToken ct)
    {
        var exercise = new Exercise
        {
            Name = command.Name,
            Description = command.Description,
            MuscleGroup = command.MuscleGroup,
            Equipment = command.Equipment,
            Difficulty = command.Difficulty,
            VideoUrl = command.VideoUrl,
            ImageUrl = command.ImageUrl,
            IsPublic = command.IsPublic,
            CreatedByUserId = userId
        };

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync(ct);
        return exercise.Id;
    }

    public async Task UpdateExerciseAsync(UpdateExerciseCommand command, Guid userId, CancellationToken ct)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == command.Id, ct)
            ?? throw new NotFoundException(ErrorCodes.ExerciseNotFound, "Exercise", command.Id);

        if (exercise.CreatedByUserId != userId)
            throw new ForbiddenException(ErrorCodes.ExerciseForbidden);

        exercise.Name = command.Name;
        exercise.Description = command.Description;
        exercise.MuscleGroup = command.MuscleGroup;
        exercise.Equipment = command.Equipment;
        exercise.Difficulty = command.Difficulty;
        exercise.VideoUrl = command.VideoUrl;
        exercise.ImageUrl = command.ImageUrl;
        exercise.IsPublic = command.IsPublic;
        exercise.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteExerciseAsync(Guid id, Guid userId, CancellationToken ct)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new NotFoundException(ErrorCodes.ExerciseNotFound, "Exercise", id);

        if (exercise.CreatedByUserId != userId)
            throw new ForbiddenException(ErrorCodes.ExerciseForbidden);

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<ExerciseListDto> GetExercisesAsync(string? muscleGroup, string? equipment, string? difficulty, int page, int pageSize, CancellationToken ct)
    {
        var query = _context.Exercises.AsQueryable();

        if (!string.IsNullOrWhiteSpace(muscleGroup))
            query = query.Where(e => e.MuscleGroup == muscleGroup);
        if (!string.IsNullOrWhiteSpace(equipment))
            query = query.Where(e => e.Equipment == equipment);
        if (!string.IsNullOrWhiteSpace(difficulty))
            query = query.Where(e => e.Difficulty == difficulty);

        var ordered = query.OrderBy(e => e.Name);
        var totalCount = await ordered.CountAsync(ct);

        var exercises = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new ExerciseListDto
        {
            Items = exercises.Select(e => new ExerciseDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                MuscleGroup = e.MuscleGroup,
                Equipment = e.Equipment,
                Difficulty = e.Difficulty,
                VideoUrl = e.VideoUrl,
                ImageUrl = e.ImageUrl,
                IsPublic = e.IsPublic,
                CreatedAt = e.CreatedAt
            }).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ExerciseDto?> GetExerciseByIdAsync(Guid id, CancellationToken ct)
    {
        var e = await _context.Exercises.FirstOrDefaultAsync(ex => ex.Id == id, ct);
        if (e is null) return null;

        return new ExerciseDto
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            MuscleGroup = e.MuscleGroup,
            Equipment = e.Equipment,
            Difficulty = e.Difficulty,
            VideoUrl = e.VideoUrl,
            ImageUrl = e.ImageUrl,
            IsPublic = e.IsPublic,
            CreatedAt = e.CreatedAt
        };
    }
}
