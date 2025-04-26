using MediatR;
using MessengerAPI.Application.Channels.Common;

namespace MessengerAPI.Application.Channels.Commands.CreateCloudAttachments;

public record CreateCloudAttachmentsCommand(
    long ChannelId,
    List<UploadAttachmentDto> Files
) : IRequest<CreateCloudAttachmentsResponse>;
