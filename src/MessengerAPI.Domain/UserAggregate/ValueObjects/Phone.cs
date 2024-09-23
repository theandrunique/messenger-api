namespace MessengerAPI.Domain.UserAggregate.ValueObjects;

public class Phone
{
    /// <summary>
    /// Phone number
    /// </summary>
    public string Data { get; private set; }
    /// <summary>
    /// Is phone verified
    /// </summary>
    public bool IsVerified { get; private set; }
    /// <summary>
    /// When phone was added
    /// </summary>
    public DateTime AddedAt { get; private set; }

    private Phone() { }
}
