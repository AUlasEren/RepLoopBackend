using FluentValidation;

namespace RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi zorunludur.")
            .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Kod zorunludur.")
            .Matches(@"^\d{6}$").WithMessage("Kod 6 haneli rakamlardan oluşmalıdır.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre zorunludur.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.");
    }
}
