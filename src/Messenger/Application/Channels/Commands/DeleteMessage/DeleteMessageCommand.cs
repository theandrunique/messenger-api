using MediatR;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.DeleteMessage;

public record DeleteMessageCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
