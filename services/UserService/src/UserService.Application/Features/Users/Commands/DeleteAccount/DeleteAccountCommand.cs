using MediatR;

namespace UserService.Application.Features.Users.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid UserId) : IRequest;
