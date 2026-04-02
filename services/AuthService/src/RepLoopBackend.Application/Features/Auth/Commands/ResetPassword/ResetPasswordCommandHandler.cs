using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IPasswordResetCodeStore _codeStore;

    public ResetPasswordCommandHandler(IIdentityService identityService, IPasswordResetCodeStore codeStore)
    {
        _identityService = identityService;
        _codeStore = codeStore;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var result = await _codeStore.VerifyCodeAsync(normalizedEmail, request.Code, cancellationToken);

        if (!result.Success)
            throw new BadRequestException(result.ErrorCode ?? ErrorCodes.ResetCodeInvalid,
                result.Error ?? "Geçersiz kod.");

        var (success, error) = await _identityService.ResetPasswordAsync(
            request.Email, result.IdentityToken!, request.NewPassword);

        if (!success)
            throw new BadRequestException(ErrorCodes.ResetPasswordFailed, error ?? "Şifre sıfırlama başarısız.");
    }
}
