using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Contracts.Events;

namespace RepLoopBackend.Application.Features.Auth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IEventPublisher _eventPublisher;

    public ForgotPasswordCommandHandler(IIdentityService identityService, IEventPublisher eventPublisher)
    {
        _identityService = identityService;
        _eventPublisher = eventPublisher;
    }

    public async Task Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var (found, resetToken) = await _identityService.GeneratePasswordResetTokenAsync(request.Email);

        if (found && resetToken != null)
        {
            // TODO: user displayName'i de geçilebilir — şimdilik email prefix kullanıyoruz
            var displayName = request.Email.Split('@')[0];

            await _eventPublisher.PublishAsync(
                new PasswordResetRequestedEvent(request.Email, displayName, resetToken),
                cancellationToken);
        }

        // Kullanıcı varlığını sızdırmamak için her zaman başarılı dön
    }
}