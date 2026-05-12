using FluentValidation;

namespace RepLoopBackend.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre zorunludur.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Ad zorunludur.")
            .MinimumLength(2).WithMessage("Ad en az 2 karakter olmalıdır.")
            .MaximumLength(100).WithMessage("Ad en fazla 100 karakter olabilir.");
    }
}
