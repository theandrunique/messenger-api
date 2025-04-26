using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.RemoveChannelMember;

public record RemoveChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
