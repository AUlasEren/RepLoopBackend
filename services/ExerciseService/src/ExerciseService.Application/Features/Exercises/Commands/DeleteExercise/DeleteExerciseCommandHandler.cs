using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.DeleteExercise;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly IExerciseDbContext _context;

    public DeleteExerciseCommandHandler(IExerciseDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteExerciseCommand request, CancellationToken ct)
    {
        var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == request.Id, ct)
            ?? throw new NotFoundException(ErrorCodes.ExerciseNotFound, "Exercise", request.Id);

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync(ct);
    }
}
