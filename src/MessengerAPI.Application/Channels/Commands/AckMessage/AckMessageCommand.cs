using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AckMessage;

public record AckMessageCommand(long ChannelId, long MessageId) : IRequest<ErrorOr<Unit>>;
