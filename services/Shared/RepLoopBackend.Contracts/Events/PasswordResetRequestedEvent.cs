namespace RepLoopBackend.Contracts.Events;

public record PasswordResetRequestedEvent(
    string Email,
    string DisplayName,
    string ResetToken);
