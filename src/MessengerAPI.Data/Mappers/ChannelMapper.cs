using Cassandra;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Mappers;

internal static class ChannelMapper
{
    public static Channel Map(Row row)
    {
        return new Channel(
            row.GetValue<long>("channelid"),
            row.GetValue<long?>("ownerid"),
            row.GetValue<string?>("title"),
            row.GetValue<Image?>("image"),
            (ChannelType)row.GetValue<int>("channeltype"),
            row.GetValue<DateTimeOffset?>("lastmessagetimestamp"),
            row.GetValue<MessageInfo?>("lastmessage")
        );
    }

    public static ChannelMemberInfo MapChannelUser(Row row)
    {
        return new ChannelMemberInfo(
            row.GetValue<long>("userid"),
            row.GetValue<string>("username"),
            row.GetValue<string>("globalname"),
            row.GetValue<Image?>("image"),
            row.GetValue<long>("readat"));
    }
}
