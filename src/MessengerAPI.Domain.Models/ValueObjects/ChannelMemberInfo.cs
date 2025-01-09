using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Domain.Models.ValueObjects;

public class ChannelMemberInfo
{
    public Guid Id { get; private set; }
    public string Username { get; init; }
    public string GlobalName { get; init; }
    public IEnumerable<Image> Images { get; init; }

    public ChannelMemberInfo(User member)
    {
        Id = member.Id;
        Username = member.Username;
        GlobalName = member.GlobalName;
        Images = member.Images.ToList();
    }

    public ChannelMemberInfo(Guid id, string username, string globalName, IEnumerable<Image> images)
    {
        Id = id;
        Username = username;
        GlobalName = globalName;
        Images = images;
    }
}
