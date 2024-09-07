using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Domain.Channel.ValueObjects;

public class MemberId
{
    public UserId UserId { get; private set; }

    public MemberId(UserId userId)
    {
        UserId = userId;
    }
}
