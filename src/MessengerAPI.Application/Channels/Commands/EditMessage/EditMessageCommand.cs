using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands.EditMessage;

public record EditMessageCommand(
    MessageId MessageId,
    UserId Sub,
    ChannelId ChannelId,
    string Text,
    MessageId? ReplyTo,
    List<Guid>? Attachments) : IRequest<ErrorOr<MessageSchema>>;
