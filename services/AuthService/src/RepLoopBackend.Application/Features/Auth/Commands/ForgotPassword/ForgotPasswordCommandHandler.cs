using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Contracts.Events;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IPasswordResetCodeStore _codeStore;

    public ForgotPasswordCommandHandler(
        IIdentityService identityService,
        IEventPublisher eventPublisher,
        IPasswordResetCodeStore codeStore)
    {
        _identityService = identityService;
        _eventPublisher = eventPublisher;
        _codeStore = codeStore;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        // Cooldown kontrolü: aynı email'e 2dk içinde tekrar kod gönderme
        if (await _codeStore.HasRecentCodeAsync(normalizedEmail, cancellationToken))
            throw new BadRequestException(ErrorCodes.ResetPasswordCooldown,
                "Lütfen yeni bir kod istemeden önce 2 dakika bekleyin.");

        var (found, resetToken) = await _identityService.GeneratePasswordResetTokenAsync(request.Email);

        if (!found || resetToken == null)
            return; // Kullanıcı varlığını sızdırmamak için sessizce dön

        var code = await _codeStore.CreateCodeAsync(normalizedEmail, resetToken, cancellationToken);

        var displayName = request.Email.Split('@')[0];

        await _eventPublisher.PublishAsync(
            new PasswordResetRequestedEvent(request.Email, displayName, code),
            cancellationToken);
    }
}
