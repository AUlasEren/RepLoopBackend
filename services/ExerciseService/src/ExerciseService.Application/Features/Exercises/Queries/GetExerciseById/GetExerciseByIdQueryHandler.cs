using MediatR;
using Microsoft.EntityFrameworkCore;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExerciseById;

public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto?>
{
    private readonly IExerciseDbContext _context;

    public GetExerciseByIdQueryHandler(IExerciseDbContext context)
    {
        _context = context;
    }

    public async Task<ExerciseDto?> Handle(GetExerciseByIdQuery request, CancellationToken ct)
    {
        var e = await _context.Exercises.FirstOrDefaultAsync(ex => ex.Id == request.Id, ct);
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
