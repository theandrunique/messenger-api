namespace MessengerAPI.Domain.UserAggregate.ValueObjects;

public class Email
{
    /// <summary>
    /// Email
    /// </summary>
    public string Data { get; private set; }
    /// <summary>
    /// Is email verified
    /// </summary>
    public bool IsVerified { get; private set; }
    /// <summary>
    /// Is email public
    /// </summary>
    public bool IsPublic { get; private set; }
    /// <summary>
    /// When email was added
    /// </summary>
    public DateTime AddedAt { get; private set; }

    private Email() { }
}
