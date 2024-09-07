using ErrorOr;
using MediatR;
using MessengerAPI.Domain.Channel.ValueObjects;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

public record CreateMessageCommand(
    UserId Sub,
    ChannelId ChannelId,
    string Text,
    MessageId? ReplyTo,
    List<Guid> FileIds
) : IRequest<ErrorOr<CreateMessageResult>>;
