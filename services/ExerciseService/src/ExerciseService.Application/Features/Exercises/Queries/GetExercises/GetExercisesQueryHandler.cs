using MediatR;
using Microsoft.EntityFrameworkCore;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExercises;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, List<ExerciseDto>>
{
    private readonly IExerciseDbContext _context;

    public GetExercisesQueryHandler(IExerciseDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken ct)
    {
        var query = _context.Exercises.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.MuscleGroup))
            query = query.Where(e => e.MuscleGroup == request.MuscleGroup);

        if (!string.IsNullOrWhiteSpace(request.Equipment))
            query = query.Where(e => e.Equipment == request.Equipment);

        if (!string.IsNullOrWhiteSpace(request.Difficulty))
            query = query.Where(e => e.Difficulty == request.Difficulty);

        var exercises = await query.OrderBy(e => e.Name).ToListAsync(ct);

        return exercises.Select(e => new ExerciseDto
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
        }).ToList();
    }
}
