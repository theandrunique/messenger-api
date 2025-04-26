using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddChannelMember;

public record AddChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
