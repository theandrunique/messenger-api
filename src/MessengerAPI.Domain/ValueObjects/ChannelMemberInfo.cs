using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Domain.ValueObjects;

public struct ChannelMemberInfo
{
    public long UserId { get; init; }
    public long ReadAt { get; init; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public Image? Image { get; init; }

    public ChannelMemberInfo(User user)
    {
        UserId = user.Id;
        Username = user.Username;
        GlobalName = user.GlobalName;
        Image = user.Image;
        ReadAt = 0;
    }

    public ChannelMemberInfo(long userId, string username, string globalName, Image? image, long readAt)
    {
        UserId = userId;
        Username = username;
        GlobalName = globalName;
        Image = image;
        ReadAt = readAt;
    }
}
