using Cassandra;
using MessengerAPI.Data.DataDto;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Mappers;

internal static class ChannelMapper
{
    public static ChannelData Map(Row row)
    {
        return new ChannelData()
        {
            Id = row.GetValue<long>("channelid"),
            OwnerId = row.GetValue<long?>("ownerid"),
            Title = row.GetValue<string?>("title"),
            Image = row.GetValue<Image?>("image"),
            Type = (ChannelType)row.GetValue<int>("channeltype"),
            LastMessageTimestamp = row.GetValue<DateTimeOffset?>("lastmessagetimestamp"),
            LastMessage = row.GetValue<MessageInfo?>("lastmessage")
        };
    }

    public static ChannelMemberInfo MapChannelUser(Row row)
    {
        return new ChannelMemberInfo(
            userId: row.GetValue<long>("userid"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("globalname"),
            image: row.GetValue<Image?>("image"),
            readAt: row.GetValue<long>("readat"),
            permissions: new ChannelPermissionSet((ulong)row.GetValue<long>("permissions")));
    }
}
