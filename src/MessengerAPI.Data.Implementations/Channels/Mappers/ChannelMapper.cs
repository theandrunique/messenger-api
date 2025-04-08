using Cassandra;
using MessengerAPI.Data.Implementations.Channels.Dto;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Implementations.Channels.Mappers;

public static class ChannelMapper
{
    public static ChannelData Map(Row row)
    {
        return new ChannelData()
        {
            Id = row.GetValue<long>("channelid"),
            OwnerId = row.GetValue<long?>("ownerid"),
            Title = row.GetValue<string?>("title"),
            Image = row.GetValue<string?>("image"),
            Type = (ChannelType)row.GetValue<int>("channeltype"),
            LastMessageTimestamp = row.GetValue<DateTimeOffset?>("lastmessagetimestamp"),
            LastMessage = row.GetValue<MessageInfoDto?>("lastmessage")
        };
    }

    public static ChannelMemberInfo MapChannelUser(Row row)
    {
        return new ChannelMemberInfo(
            userId: row.GetValue<long>("userid"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("globalname"),
            image: row.GetValue<string?>("image"),
            lastReadMessageId: row.GetValue<long>("lastreadmessageid"),
            permissions: new ChannelPermissionSet((ulong)row.GetValue<long>("permissions")),
            isLeave: row.GetValue<bool>("isleave"));
    }
}
