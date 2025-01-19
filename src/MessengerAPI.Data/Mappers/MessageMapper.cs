using Cassandra;
using MessengerAPI.Domain.Entities.ValueObjects;
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
}