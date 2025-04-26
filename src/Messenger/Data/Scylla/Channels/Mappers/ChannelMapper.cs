using Cassandra;
using Messenger.Data.Scylla.Channels.Dto;
using Messenger.Domain.Channels;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Mappers;

public static class ChannelMapper
{
    public static ChannelData Map(Row row)
    {
        var permissionOverwrites = row.GetValue<long?>("permissionoverwrites");

        return new ChannelData()
        {
            Id = row.GetValue<long>("channelid"),
            OwnerId = row.GetValue<long?>("ownerid"),
            Name = row.GetValue<string?>("name"),
            Image = row.GetValue<string?>("image"),
            Type = (ChannelType)row.GetValue<int>("channeltype"),
            LastMessageTimestamp = row.GetValue<DateTimeOffset?>("lastmessagetimestamp"),
            LastMessage = row.GetValue<MessageInfoDto?>("lastmessage"),
            PermissionOverwrites = permissionOverwrites.HasValue
                ? new ChannelPermissionSet((ulong)permissionOverwrites.Value)
                : null,
        };
    }

    public static ChannelMemberInfo MapChannelUser(Row row)
    {
        var permissionOverwrites = row.GetValue<long?>("permissionoverwrites");

        return new ChannelMemberInfo(
            userId: row.GetValue<long>("userid"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("globalname"),
            image: row.GetValue<string?>("image"),
            lastReadMessageId: row.GetValue<long>("lastreadmessageid"),
            permissionOverwrites: permissionOverwrites.HasValue
                ? new ChannelPermissionSet((ulong)permissionOverwrites.Value)
                : null,
            isLeave: row.GetValue<bool>("isleave"));
    }
}
