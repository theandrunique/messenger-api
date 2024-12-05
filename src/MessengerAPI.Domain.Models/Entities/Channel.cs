using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Relations;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Domain.Models.Entities;

public class Channel
{
    public Guid Id { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public Image? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public long? LastMessageId { get; private set; }

    public IReadOnlyList<ChannelMember> Members => _members.ToList();
    private readonly List<ChannelMember> _members = new();

    public Channel(
        ChannelType type,
        Guid? ownerId = null,
        string? title = null,
        Image? image = null)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
    }

    public void AddMember(Guid userId)
    {
        _members.Add(new ChannelMember(userId, Id));
    }

    public void SetMembers(IEnumerable<ChannelMember> members)
    {
        _members.Clear();
        _members.AddRange(members);
    }

    public bool IsUserInTheChannel(Guid userId)
    {
        return _members.Any(m => m.UserId == userId);
    }
}
