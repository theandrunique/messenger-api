using MessengerAPI.Domain.Chat.Entities;
using MessengerAPI.Domain.Chat.ValueObjects;
using MessengerAPI.Domain.Common.Entities;

namespace MessengerAPI.Domain.Common.ValueObjects;

public class Chat
{
    public Guid Id { get; private set; }
    public Guid? OwnerId { get; private set; }
    public string? Title { get; private set; }
    public FileData? ChatPhoto { get; private set; }
    public ChatType Type { get; private set; }
    public List<Guid> MemberIds { get; private set; }
    public List<Guid> AdminIds { get; private set; }
    public Message? LastMessage { get; private set; }
    public List<int> PinnedMessageIds { get; private set; }

    public Chat()
    {
    }
}
