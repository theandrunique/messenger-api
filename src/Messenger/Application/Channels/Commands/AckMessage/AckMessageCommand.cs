using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.AckMessage;

public record AckMessageCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
