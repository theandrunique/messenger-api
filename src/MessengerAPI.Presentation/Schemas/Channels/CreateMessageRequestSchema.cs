using MessengerAPI.Application.Channels.Common;

namespace MessengerAPI.Presentation.Schemas.Channels;

/// <summary>
/// Request schema for create message
/// </summary>
public record CreateMessageRequestSchema
{
    public string content { get; init; }
    public List<MessageAttachmentDto>? attachments { get; init; }
}
