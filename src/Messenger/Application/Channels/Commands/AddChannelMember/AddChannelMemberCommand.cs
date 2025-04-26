using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.AddChannelMember;

public record AddChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
