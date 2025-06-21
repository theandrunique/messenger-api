using Cassandra;
using Messenger.Data.Scylla.Common;
using Messenger.Data.Scylla.Messages.Dto;
using Messenger.Domain.Messages.ValueObjects;

namespace Messenger.Data.Scylla.Messages.Mappers;

public static class MessageMapper
{
    public static MessageAuthorInfo MapMessageAuthorInfo(Row row)
    {
        return new MessageAuthorInfo(
            id: row.GetValue<long>("user_id"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("global_name"),
            image: row.GetValue<string?>("image")
        );
    }

    public static MessageData Map(Row row)
    {
        return new MessageData()
        {
            Id = row.GetValue<long>("message_id"),
            ChannelId = row.GetValue<long>("channel_id"),
            AuthorId = row.GetValue<long>("author_id"),
            TargetUserId = row.GetValue<long?>("target_user_id"),
            Content = row.GetValue<string>("content"),
            Timestamp = row.GetValue<DateTimeOffset>("timestamp"),
            EditedTimestamp = row.GetValue<DateTimeOffset?>("edited_timestamp"),
            Pinned = row.GetValue<bool>("pinned"),
            Type = (MessageType)row.GetValue<int>("type"),
            ReferencedMessageId = row.GetValue<long?>("referenced_message_id"),
            Metadata = MessageMetadataConverter.ToValue(row.GetValue<string?>("metadata")),
        };
    }
}