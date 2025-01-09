using System.ComponentModel.DataAnnotations.Schema;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Data.Tables;

internal class ChannelUsers
{
    public Guid UserId { get; set; }
    public Guid ChannelId { get; set; }
    public Guid? ReadAt { get; set; }
    public string Username { get; set; }
    public string GlobalName { get; set; }
    public IEnumerable<Image> Images { get; set; }

    public static List<ChannelUsers> FromChannel(Channel channel)
    {
        var list = new List<ChannelUsers>();
        foreach (var member in channel.Members)
        {
            list.Add(new ChannelUsers()
            {
                UserId = member.Id,
                ChannelId = channel.Id,
                Username = member.Username,
                GlobalName = member.GlobalName,
                Images = member.Images.ToList()
            });
        }
        return list;
    }

    public static ChannelUsers FromMember(ChannelMemberInfo member, Guid channelId)
    {
        return new ChannelUsers()
        {
            UserId = member.Id,
            ChannelId = channelId,
            Username = member.Username,
            GlobalName = member.GlobalName,
            Images = member.Images.ToList()
        };
    }

    public ChannelMemberInfo ToChannelMemberInfo()
        => new ChannelMemberInfo(UserId, Username, GlobalName, Images);
}
