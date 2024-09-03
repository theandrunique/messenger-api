namespace MessengerAPI.Domain.User.ValueObjects;

public class Phone
{
    public string Data { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime AddedAt { get; private set; }

    public Phone() { }
}
