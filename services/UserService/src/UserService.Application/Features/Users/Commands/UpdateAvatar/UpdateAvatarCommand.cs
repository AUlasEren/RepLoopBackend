using MediatR;

namespace UserService.Application.Features.Users.Commands.UpdateAvatar;

public record UpdateAvatarCommand(Guid UserId, Stream FileStream, string ContentType, string FileName) : IRequest<string>;
