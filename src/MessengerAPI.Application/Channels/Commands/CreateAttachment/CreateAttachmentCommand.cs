using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.CreateAttachment;

public record CreateAttachmentCommand(
    long ChannelId,
    IEnumerable<UploadAttachmentDto> Files
) : IRequest<ErrorOr<List<CloudAttachmentSchema>>>;
