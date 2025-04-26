using MediatR;

namespace Messenger.Application.Users.Commands.RemoveAvatar;

public record RemoveAvatarCommand : IRequest<Unit>;
