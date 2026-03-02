using MediatR;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Domain.Entities;

namespace ExerciseService.Application.Features.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
{
    private readonly IExerciseDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateExerciseCommandHandler(IExerciseDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateExerciseCommand request, CancellationToken ct)
    {
        var exercise = new Exercise
        {
            Name = request.Name,
            Description = request.Description,
            MuscleGroup = request.MuscleGroup,
            Equipment = request.Equipment,
            Difficulty = request.Difficulty,
            VideoUrl = request.VideoUrl,
            ImageUrl = request.ImageUrl,
            IsPublic = request.IsPublic,
            CreatedByUserId = _currentUserService.UserId
        };

        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync(ct);
        return exercise.Id;
    }
}
