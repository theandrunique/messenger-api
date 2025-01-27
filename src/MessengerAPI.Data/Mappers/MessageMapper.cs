using Cassandra;
using MessengerAPI.Data.DataDto;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Mappers;

internal static class MessageMapper
{
    public static MessageAuthorInfo MapMessageAuthorInfo(Row row)
    {
        return new MessageAuthorInfo(
            id: row.GetValue<long>("userid"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("globalname"),
            image: row.GetValue<Image?>("image")
        );
    }

    public static MessageData Map(Row row)
    {
        return new MessageData()
        {
            Id = row.GetValue<long>("id"),
            ChannelId = row.GetValue<long>("channelid"),
            AuthorId = row.GetValue<long>("authorid"),
            Content = row.GetValue<string>("content"),
            Timestamp = row.GetValue<DateTimeOffset>("timestamp"),
            EditedTimestamp = row.GetValue<DateTimeOffset?>("editedtimestamp"),
            Pinned = row.GetValue<bool>("pinned"),
            Type = (MessageType)row.GetValue<int>("type"),
        };
    }
}