using FluentValidation;

namespace SessionService.Application.Features.Sessions.Commands.StartSession;

public class StartSessionCommandValidator : AbstractValidator<StartSessionCommand>
{
    public StartSessionCommandValidator()
    {
        RuleFor(x => x.WorkoutId).NotEmpty();
        RuleFor(x => x.WorkoutName).NotEmpty().MaximumLength(200);
    }
}
