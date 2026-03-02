using MediatR;
using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Features.Auth.Commands.AppleAuth;

public record AppleAuthCommand(string IdentityToken, string? FullName) : IRequest<AuthResult>;