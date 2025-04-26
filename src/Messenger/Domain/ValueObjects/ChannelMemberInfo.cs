using Messenger.Domain.Channels;
using Messenger.Domain.Entities;

namespace Messenger.Domain.ValueObjects;

public class ChannelMemberInfo
{
    public long UserId { get; set; }
    public long LastReadMessageId { get; set; }
    public ChannelPermissionSet? PermissionOverwrites { get; set; }
    public string Username { get; set; }
    public string GlobalName { get; set; }
    public string? Image { get; set; }
    public bool IsLeave { get; set; }

    public ChannelMemberInfo(User user)
    {
        UserId = user.Id;
        Username = user.Username;
        GlobalName = user.GlobalName;
        Image = user.Image;
        LastReadMessageId = 0;
        IsLeave = false;
    }

    public ChannelMemberInfo(
        long userId,
        string username,
        string globalName,
        string? image,
        long lastReadMessageId,
        ChannelPermissionSet? permissionOverwrites,
        bool isLeave)
    {
        UserId = userId;
        Username = username;
        GlobalName = globalName;
        Image = image;
        LastReadMessageId = lastReadMessageId;
        PermissionOverwrites = permissionOverwrites;
        IsLeave = isLeave;
    }

    public void SetLeaveStatus(bool newStatus)
    {
        if (IsLeave == newStatus)
        {
            throw new InvalidOperationException($"Status '{newStatus}' (IsLeave) of user id{UserId} already set.");
        }

        IsLeave = newStatus;
    }
}
