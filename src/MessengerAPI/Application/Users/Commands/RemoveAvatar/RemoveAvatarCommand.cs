using MediatR;

namespace MessengerAPI.Application.Users.Commands.RemoveAvatar;

public record RemoveAvatarCommand : IRequest<Unit>;
