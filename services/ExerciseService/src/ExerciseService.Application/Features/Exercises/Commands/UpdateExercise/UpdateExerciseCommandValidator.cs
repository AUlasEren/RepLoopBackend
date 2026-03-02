using FluentValidation;

namespace ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    public UpdateExerciseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
