using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AddChannelMember;

public record AddChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
