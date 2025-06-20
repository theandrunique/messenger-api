using Cassandra;
using Messenger.Data.Scylla.Common;
using Messenger.Data.Scylla.Messages.Dto;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.Messages.Mappers;

public static class MessageMapper
{
    public static MessageAuthorInfo MapMessageAuthorInfo(Row row)
    {
        return new MessageAuthorInfo(
            id: row.GetValue<long>("userid"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("globalname"),
            image: row.GetValue<string?>("image")
        );
    }

    public static MessageData Map(Row row)
    {
        return new MessageData()
        {
            Id = row.GetValue<long>("id"),
            ChannelId = row.GetValue<long>("channelid"),
            AuthorId = row.GetValue<long>("authorid"),
            TargetUserId = row.GetValue<long?>("targetuserid"),
            Content = row.GetValue<string>("content"),
            Timestamp = row.GetValue<DateTimeOffset>("timestamp"),
            EditedTimestamp = row.GetValue<DateTimeOffset?>("editedtimestamp"),
            Pinned = row.GetValue<bool>("pinned"),
            Type = (MessageType)row.GetValue<int>("type"),
            ReferencedMessageId = row.GetValue<long?>("referencedmessageid"),
            Metadata = MessageMetadataConverter.ToValue(row.GetValue<string?>("metadata")),
        };
    }
}