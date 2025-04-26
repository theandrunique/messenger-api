using MediatR;
using Messenger.Application.Channels.Common;

namespace Messenger.Application.Channels.Commands.CreateCloudAttachments;

public record CreateCloudAttachmentsCommand(
    long ChannelId,
    List<UploadAttachmentDto> Files
) : IRequest<CreateCloudAttachmentsResponse>;
