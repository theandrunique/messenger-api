using MessengerAPI.Application.Channels.Common;

namespace MessengerAPI.Presentation.Schemas.Channels;

/// <summary>
/// Request schema for create message
/// </summary>
public record CreateMessageRequestSchema
{
    /// <summary>
    /// Text of the message
    /// </summary>
    public string text { get; init; }
    /// <summary>
    /// Id of the message to reply
    /// </summary>
    public long? replyTo { get; init; }
    /// <summary>
    /// List of file id to attach
    /// </summary>
    public List<FileData2>? attachments { get; init; }
}
