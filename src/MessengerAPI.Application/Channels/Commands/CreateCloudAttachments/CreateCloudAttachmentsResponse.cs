using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Channels.Commands.CreateCloudAttachments;

public record CreateCloudAttachmentsResponse(
    List<CloudAttachmentSchema> Results,
    List<CloudAttachmentErrorSchema> Errors);
