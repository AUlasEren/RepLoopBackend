using MediatR;
using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string DisplayName) : IRequest<AuthResult>;
