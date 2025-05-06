using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.DeleteMessage;

public record DeleteMessageCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
