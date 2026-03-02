using MediatR;

namespace RepLoopBackend.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;