using Messenger.Application.Channels.Common;

namespace Messenger.Presentation.Schemas.Channels;

/// <summary>
/// Request schema for create message
/// </summary>
public record CreateMessageRequestSchema(
    string content,
    List<MessageAttachmentDto>? attachments);
