using Messenger.Data.Scylla.Common;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Messages.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Dto;

public struct MessageInfoDto
{
    public long Id { get; init; }
    public long AuthorId { get; init; }
    public long? TargetUserId { get; init; }
    public string Content { get; init; }
    public DateTimeOffset Timestamp { get; init; }
    public DateTimeOffset? EditedTimestamp { get; init; }
    public int AttachmentsCount { get; init; }
    public int Type { get; init; }
    public string? Metadata { get; init; }

    public static MessageInfoDto From(MessageInfo messageInfo)
    {
        return new MessageInfoDto()
        {
            Id = messageInfo.Id,
            AuthorId = messageInfo.AuthorId,
            TargetUserId = messageInfo.TargetUserId,
            Content = messageInfo.Content,
            Timestamp = messageInfo.Timestamp,
            EditedTimestamp = messageInfo.EditedTimestamp,
            AttachmentsCount = messageInfo.AttachmentsCount,
            Type = (int)messageInfo.Type,
            Metadata = MessageMetadataConverter.ToJson(messageInfo.Metadata),
        };
    }

    public MessageInfo ToValue()
    {
        return new MessageInfo()
        {
            Id = Id,
            AuthorId = AuthorId,
            TargetUserId = TargetUserId,
            Content = Content,
            Timestamp = Timestamp,
            EditedTimestamp = EditedTimestamp,
            AttachmentsCount = AttachmentsCount,
            Type = (MessageType)Type,
            Metadata = MessageMetadataConverter.ToValue(Metadata),
        };
    }
}
