using Messenger.Contracts.Common;

namespace Messenger.Application.Channels.Commands.CreateCloudAttachments;

public record CreateCloudAttachmentsResponse(
    List<CloudAttachmentSchema> Results,
    List<CloudAttachmentErrorSchema> Errors);
