using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

public record AddOrEditMessageCommand(
    long? MessageId,
    long Sub,
    long ChannelId,
    string Content,
    List<MessageAttachmentDto>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
