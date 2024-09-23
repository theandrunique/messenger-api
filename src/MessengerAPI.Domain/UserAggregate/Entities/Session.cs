using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Domain.UserAggregate.Entities;

public class Session
{
    /// <summary>
    /// Session id
    /// </summary>
    public SessionId Id { get; private set; }
    /// <summary>
    /// User id
    /// </summary>
    public UserId UserId { get; private set; }
    /// <summary>
    /// User
    /// </summary>
    public User? User { get; private set; }
    /// <summary>
    /// Token id
    /// </summary>
    public Guid TokenId { get; private set; }
    /// <summary>
    /// Device name
    /// </summary>
    public string DeviceName { get; private set; }
    /// <summary>
    /// Client name
    /// </summary>
    public string ClientName { get; private set; }
    /// <summary>
    /// Location
    /// </summary>
    public string Location { get; private set; }
    /// <summary>
    /// DateTime of last refresh
    /// </summary>
    public DateTime LastUsedAt { get; private set; }
    /// <summary>
    /// DateTime of creation
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Update token id
    /// </summary>
    public void UpdateTokenId()
    {
        LastUsedAt = DateTime.UtcNow;
        TokenId = Guid.NewGuid();
    }

    /// <summary>
    /// Create a new session
    /// </summary>
    /// <param name="deviceName">Device name</param>
    /// <param name="clientName">Client name</param>
    /// <param name="location">Location</param>
    /// <returns></returns>
    public static Session CreateNew(string deviceName, string clientName, string location)
    {
        var session = new Session
        {
            Id = new SessionId(Guid.NewGuid()),
            TokenId = Guid.NewGuid(),
            DeviceName = deviceName,
            ClientName = clientName,
            Location = location,
            LastUsedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
        };
        return session;
    }

    private Session() { }
}
