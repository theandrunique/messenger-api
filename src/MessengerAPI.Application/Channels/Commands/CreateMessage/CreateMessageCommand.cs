using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

/// <summary>
/// Create a new message command
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
/// <param name="ChannelId">Channel id</param>
/// <param name="Text">Message text</param>
/// <param name="ReplyTo">Message id to reply</param>
/// <param name="Attachments">List of file ids to attach</param>
public record CreateMessageCommand(
    UserId Sub,
    ChannelId ChannelId,
    string Text,
    MessageId? ReplyTo,
    List<Guid>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
