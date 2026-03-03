using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand>
{
    private readonly IExerciseDbContext _context;

    public UpdateExerciseCommandHandler(IExerciseDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateExerciseCommand request, CancellationToken ct)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, ct)
            ?? throw new NotFoundException(ErrorCodes.ExerciseNotFound, "Exercise", request.Id);

        exercise.Name = request.Name;
        exercise.Description = request.Description;
        exercise.MuscleGroup = request.MuscleGroup;
        exercise.Equipment = request.Equipment;
        exercise.Difficulty = request.Difficulty;
        exercise.VideoUrl = request.VideoUrl;
        exercise.ImageUrl = request.ImageUrl;
        exercise.IsPublic = request.IsPublic;
        exercise.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
    }
}
