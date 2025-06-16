using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.RemoveChannelMember;

public record RemoveChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
