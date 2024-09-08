using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.ChannelAggregate.ValueObjects;

public class MemberId
{
    public UserId UserId { get; private set; }

    public MemberId(UserId userId)
    {
        UserId = userId;
    }
}
