using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessagePrivateChannel;

public record AddOrEditMessagePrivateChannelCommand(
    long userId,
    long? MessageId,
    long ChannelId,
    string Content,
    List<MessageAttachmentDto>? Attachments
) : IRequest<ErrorOr<MessageSchema>>;
