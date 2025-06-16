using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AddOrEditMessage;

public record AddOrEditMessageCommand(
    long? MessageId,
    long? ReferencedMessageId,
    long ChannelId,
    string Content,
    List<MessageAttachmentDto>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
