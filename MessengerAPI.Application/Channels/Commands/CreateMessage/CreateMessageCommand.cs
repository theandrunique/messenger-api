using ErrorOr;
using MediatR;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

public record CreateMessageCommand(
    UserId Sub,
    ChannelId ChannelId,
    string Text,
    MessageId? ReplyTo,
    List<Guid> FileIds
) : IRequest<ErrorOr<CreateMessageResult>>;
