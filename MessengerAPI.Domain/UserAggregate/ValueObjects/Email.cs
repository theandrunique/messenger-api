namespace MessengerAPI.Domain.UserAggregate.ValueObjects;

public class Email
{
    public string Data { get; private set; }
    public bool IsVerified { get; private set; }
    public bool IsPublic { get; private set; }
    public DateTime AddedAt { get; private set; }

    private Email() { }
}
