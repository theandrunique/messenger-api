namespace MessengerAPI.Domain.Models.Relations;

public class ChannelMember
{
    public Guid UserId { get; private set; }
    public Guid ChannelId { get; private set; }

    public ChannelMember(Guid userId, Guid channelId)
    {
        UserId = userId;
        ChannelId = channelId;
    }
}
