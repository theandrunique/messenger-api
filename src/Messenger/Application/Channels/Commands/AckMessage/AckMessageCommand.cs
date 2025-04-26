using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AckMessage;

public record AckMessageCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
