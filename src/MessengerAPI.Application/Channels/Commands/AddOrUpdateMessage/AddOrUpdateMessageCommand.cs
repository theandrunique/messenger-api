using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

/// <summary>
/// Add or update a message command
/// </summary>
/// <param name="MessageId">Message id, null when message is new</param>
/// <param name="Sub">User id</param>
/// <param name="ChannelId">Channel id</param>
/// <param name="Text">Message text</param>
/// <param name="ReplyTo">Message id to reply</param>
/// <param name="Attachments">List of file ids to attach</param>
public record AddOrUpdateMessageCommand(
    long? MessageId,
    Guid Sub,
    Guid ChannelId,
    string Text,
    long? ReplyTo,
    List<Guid>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
