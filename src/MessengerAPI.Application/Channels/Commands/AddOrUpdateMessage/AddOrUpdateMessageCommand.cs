using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public record AddOrUpdateMessageCommand(
    long? MessageId,
    long Sub,
    long ChannelId,
    string Content,
    long? ReplyTo,
    List<MessageAttachmentDto>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
