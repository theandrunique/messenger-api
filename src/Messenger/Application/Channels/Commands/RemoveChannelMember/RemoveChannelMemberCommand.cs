using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.RemoveChannelMember;

public record RemoveChannelMemberCommand(long ChannelId, long UserId) : IRequest<ErrorOr<Unit>>;
