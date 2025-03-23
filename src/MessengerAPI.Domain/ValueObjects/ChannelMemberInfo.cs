using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.ValueObjects;

public class ChannelMemberInfo
{
    public long UserId { get; set; }
    public long ReadAt { get; set; }
    public ChannelPermissionSet Permissions { get; set; }
    public string Username { get; set; }
    public string GlobalName { get; set; }
    public string? Image { get; set; }
    public bool IsLeave { get; set; }

    public ChannelMemberInfo(User user, ChannelPermissions permissions)
    {
        UserId = user.Id;
        Username = user.Username;
        GlobalName = user.GlobalName;
        Permissions = new ChannelPermissionSet(permissions);
        Image = user.Image;
        ReadAt = 0;
        IsLeave = false;
    }

    public ChannelMemberInfo(
        long userId,
        string username,
        string globalName,
        string? image,
        long readAt,
        ChannelPermissionSet permissions,
        bool isLeave)
    {
        UserId = userId;
        Username = username;
        GlobalName = globalName;
        Image = image;
        ReadAt = readAt;
        Permissions = permissions;
        IsLeave = isLeave;
    }

    public void SetLeaveStatus(bool newStatus)
    {
        if (IsLeave == newStatus)
        {
            throw new InvalidOperationException($"Status '{newStatus}' (IsLeave) of user {UserId} already set.");
        }

        IsLeave = newStatus;
    }
}
