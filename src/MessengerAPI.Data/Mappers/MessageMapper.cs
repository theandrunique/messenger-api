using Cassandra;
using MessengerAPI.Data.DataDto;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;
using Newtonsoft.Json;

namespace MessengerAPI.Data.Mappers;

internal static class MessageMapper
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
        var metadataJson = row.GetValue<string?>("metadata");
        var metadata = JsonConvert.DeserializeObject<IMessageMetadata?>(metadataJson, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });

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
            Metadata = metadata,
        };
    }
}