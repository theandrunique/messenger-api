using System.Text.Json.Serialization;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Contracts.Common;

public record MessageInfoSchema
{
    public string Id { get; init; }
    public UserPublicSchema Author { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public UserPublicSchema? TargetUser { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public int AttachmentsCount { get; init; }
    public MessageType Type { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Metadata { get; init; }

    private MessageInfoSchema(
        MessageInfo messageInfo,
        UserPublicSchema author,
        UserPublicSchema? targetUser = null)
    {
        Id = messageInfo.Id.ToString();
        Author = author;
        TargetUser = targetUser;
        Content = messageInfo.Content;
        Timestamp = messageInfo.Timestamp;
        EditedTimestamp = messageInfo.EditedTimestamp;
        AttachmentsCount = messageInfo.AttachmentsCount;
        Type = messageInfo.Type;
        Metadata = messageInfo.Metadata;
    }

    public static MessageInfoSchema From(
        MessageInfo messageInfo,
        UserPublicSchema author,
        UserPublicSchema? targetUser) => new(messageInfo, author, targetUser);
}
