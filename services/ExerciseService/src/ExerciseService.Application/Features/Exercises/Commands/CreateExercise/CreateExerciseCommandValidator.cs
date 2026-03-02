using FluentValidation;

namespace ExerciseService.Application.Features.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200);
        RuleFor(x => x.Difficulty)
            .Must(d => d is null || new[] { "Beginner", "Intermediate", "Advanced" }.Contains(d))
            .WithMessage("Difficulty must be Beginner, Intermediate, or Advanced.");
    }
}
