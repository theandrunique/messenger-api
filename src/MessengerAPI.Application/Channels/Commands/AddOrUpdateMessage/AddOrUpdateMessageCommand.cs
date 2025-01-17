using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

/// <summary>
/// Add or update a message command
/// </summary>
/// <param name="MessageId">Message id, null when message is new</param>
/// <param name="Sub">User id</param>
/// <param name="ChannelId">Channel id</param>
/// <param name="Content">Message text</param>
/// <param name="ReplyTo">Message id to reply</param>
/// <param name="Attachments">List of file ids to attach</param>
public record AddOrUpdateMessageCommand(
    long? MessageId,
    long Sub,
    long ChannelId,
    string Content,
    long? ReplyTo,
    List<MessageAttachmentDto>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
