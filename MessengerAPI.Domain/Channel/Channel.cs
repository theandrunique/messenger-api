using MessengerAPI.Domain.Channel.Entities;
using MessengerAPI.Domain.Channel.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Channel;

public class Channel
{
    private readonly List<Message> _messages = new();
    private readonly List<MemberId> _memberIds = new();
    private readonly List<AdminId> _adminIds = new();
    private readonly List<PinnedMessageId> _pinnedMessageIds = new();

    public IReadOnlyCollection<Message> Messages => _messages.ToList();
    public IReadOnlyCollection<MemberId> MemberIds => _memberIds.ToList();
    public IReadOnlyCollection<AdminId> AdminIds => _adminIds.ToList();
    public IReadOnlyCollection<PinnedMessageId> PinnedMessageIds => _pinnedMessageIds.ToList();

    public ChannelId Id { get; private set; }
    public UserId? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public FileData? Image { get; private set; }
    public ChannelType Type { get; private set; }
    public MessageId? LastMessageId { get; private set; }

    public static Channel CreatePrivate(UserId userId1, UserId userId2)
    {
        var memberIds = new List<MemberId> { new MemberId(userId1), new MemberId(userId2) };

        return new Channel(ChannelType.Private, memberIds);
    }

    public static Channel CreateGroup(UserId ownerId, List<UserId> memberIds, string? title = null, FileData? image = null)
    {
        var memberIdsConverted = memberIds.ConvertAll(memberId => new MemberId(memberId));

        return new Channel(ChannelType.Group, memberIdsConverted, ownerId, title, image);
    }

    private Channel(ChannelType type, List<MemberId> memberIds, UserId? ownerId = null, string? title = null, FileData? image = null)
    {
        Id = new ChannelId(Guid.NewGuid());
        OwnerId = ownerId;
        Title = title;
        Image = image;
        Type = type;
        _memberIds = memberIds;
    }

    public Channel() { }

    public Message AddMessage(UserId senderId, string text, MessageId? replyTo = null, List<FileData>? attachments = null)
    {
        var newMessage = Message.CreateNew(Id, senderId, text, replyTo, attachments);
        _messages.Add(newMessage);
        return newMessage;
    }
}
