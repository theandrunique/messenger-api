using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;

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
    Guid Sub,
    Guid ChannelId,
    string Text,
    long? ReplyTo,
    List<Guid>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
