using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.MessageAck;

public record MessageAckCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
