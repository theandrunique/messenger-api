using Cassandra;
using Messenger.Data.Scylla.Channels.Dto;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;

namespace Messenger.Data.Scylla.Channels.Mappers;

public static class ChannelMapper
{
    public static ChannelData Map(Row row)
    {
        var permissionOverwrites = row.GetValue<long?>("permission_overwrites");

        return new ChannelData()
        {
            Id = row.GetValue<long>("channel_id"),
            OwnerId = row.GetValue<long?>("owner_id"),
            Name = row.GetValue<string?>("name"),
            Image = row.GetValue<string?>("image"),
            Type = (ChannelType)row.GetValue<int>("type"),
            LastMessageTimestamp = row.GetValue<DateTimeOffset?>("last_message_timestamp"),
            LastMessage = row.GetValue<MessageInfoDto?>("last_message"),
            PermissionOverwrites = permissionOverwrites.HasValue
                ? new ChannelPermissionSet((ulong)permissionOverwrites.Value)
                : null,
        };
    }

    public static ChannelMemberInfo MapChannelUser(Row row)
    {
        var permissionOverwrites = row.GetValue<long?>("permission_overwrites");

        return new ChannelMemberInfo(
            userId: row.GetValue<long>("user_id"),
            username: row.GetValue<string>("username"),
            globalName: row.GetValue<string>("global_name"),
            image: row.GetValue<string?>("image"),
            lastReadMessageId: row.GetValue<long>("last_read_message_id"),
            permissionOverwrites: permissionOverwrites.HasValue
                ? new ChannelPermissionSet((ulong)permissionOverwrites.Value)
                : null,
            isLeave: row.GetValue<bool>("is_leave"));
    }
}
