using Cassandra;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Mappers;

public static class MessageMapper
{
    public static MessageSenderInfo MapMessageSenderInfo(Row row)
    {
        return new MessageSenderInfo(
            row.GetValue<long>("userid"),
            row.GetValue<string>("username"),
            row.GetValue<string>("globalname"),
            row.GetValue<Image?>("image")
        );
    }

    public static Message Map(Row row)
    {
        return new Message(
            row.GetValue<long>("channelid"),
            row.GetValue<long>("id"),
            row.GetValue<long>("authorid"),
            row.GetValue<string>("content"),
            row.GetValue<DateTimeOffset>("timestamp"),
            row.GetValue<DateTimeOffset?>("editedtimestamp"),
            row.GetValue<long?>("replyto"),
            row.GetValue<bool>("pinned"),
            (MessageType)row.GetValue<int>("type"),
            row.GetValue<List<Attachment>?>("attachments")
        );
    }
}