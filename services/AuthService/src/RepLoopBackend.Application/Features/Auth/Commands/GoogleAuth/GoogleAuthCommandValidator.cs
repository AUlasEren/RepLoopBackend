using FluentValidation;

namespace RepLoopBackend.Application.Features.Auth.Commands.GoogleAuth;

public class GoogleAuthCommandValidator : AbstractValidator<GoogleAuthCommand>
{
    public GoogleAuthCommandValidator()
    {
        RuleFor(x => x.IdToken).NotEmpty();
    }
}