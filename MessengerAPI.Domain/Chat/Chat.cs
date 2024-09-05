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

    public Chat()
    {
    }
}
