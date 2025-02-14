using MessengerAPI.Application.Channels.Common;

namespace MessengerAPI.Presentation.Schemas.Channels;

/// <summary>
/// Request schema for create message
/// </summary>
public record CreateMessageRequestSchema(
    string content,
    List<MessageAttachmentDto>? attachments);
