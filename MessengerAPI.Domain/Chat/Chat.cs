using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Common.ValueObjects;

public class Chat
{
    private readonly List<Message> _messages = new();
    private readonly List<MemberId> _memberIds = new();
    private readonly List<AdminId> _adminIds = new();
    private readonly List<PinnedMessageId> _pinnedMessageIds = new();

    public IReadOnlyCollection<Message> Messages => _messages.ToList();
    public IReadOnlyCollection<MemberId> MemberIds => _memberIds.ToList();
    public IReadOnlyCollection<AdminId> AdminIds => _adminIds.ToList();
    public IReadOnlyCollection<PinnedMessageId> PinnedMessageIds => _pinnedMessageIds.ToList();

    public ChatId Id { get; private set; }
    public UserId? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public FileData? ChatPhoto { get; private set; }
    public ChatType Type { get; private set; }
    public MessageId? LastMessageId { get; private set; }

    public static Chat CreatePrivate(UserId userId1, UserId userId2)
    {
        var memberIds = new List<MemberId> { new MemberId(userId1), new MemberId(userId2) };

        return new Chat(ChatType.Private, memberIds);
    }

    public static Chat CreateGroup(UserId ownerId, List<UserId> memberIds, string? title = null, FileData? chatPhoto = null)
    {
        var memberIdsConverted = memberIds.ConvertAll(memberId => new MemberId(memberId));

        return new Chat(ChatType.Group, memberIdsConverted, ownerId, title, chatPhoto);
    }

    private Chat(ChatType type, List<MemberId> memberIds, UserId? ownerId = null, string? title = null, FileData? chatPhoto = null)
    {
        Id = new ChatId(Guid.NewGuid());
        OwnerId = ownerId;
        Title = title;
        ChatPhoto = chatPhoto;
        Type = type;
        _memberIds = memberIds;
    }

    public Chat() { }

    public Message AddMessage(UserId senderId, string text, MessageId? replyTo = null, List<FileData>? attachments = null)
    {
        var newMessage = Message.CreateNew(Id, senderId, text, replyTo, attachments);
        _messages.Add(newMessage);
        return newMessage;
    }
}
