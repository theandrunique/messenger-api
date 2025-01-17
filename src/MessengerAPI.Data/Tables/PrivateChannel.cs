using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Tables;

internal class PrivateChannel
{
    public long UserId1 { get; set; }
    public long UserId2 { get; set; }
    public long ChannelId { get; set; }

    public static PrivateChannel FromChannel(Channel channel)
    {
        if (channel.Members == null || channel.Members.Count != 2)
            throw new ArgumentException("Private channel must have exactly two members.");

        var sortedMembers = channel.Members.OrderBy(member => member.Id).ToList();

        return new PrivateChannel
        {

            ChannelId = channel.Id,
            UserId1 = sortedMembers[0].Id,
            UserId2 = sortedMembers[1].Id,
        };
    }
}
