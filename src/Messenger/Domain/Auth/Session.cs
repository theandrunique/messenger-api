namespace Messenger.Domain.Auth;

public class Session
{
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public Guid TokenId { get; private set; }
    public string DeviceName { get; private set; }
    public string ClientName { get; private set; }
    public string Location { get; private set; }
    public DateTimeOffset LastRefreshTimestamp { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    public Session(long id, long userId, string deviceName, string clientName, string location)
    {
        var timestamp = DateTimeOffset.UtcNow;
        Id = id;
        UserId = userId;
        TokenId = Guid.NewGuid();
        DeviceName = deviceName;
        ClientName = clientName;
        Location = location;
        LastRefreshTimestamp = timestamp;
        Timestamp = timestamp;
    }

    public Session(
        long id,
        long userId,
        Guid tokenId,
        string deviceName,
        string clientName,
        string location,
        DateTimeOffset lastUsedTimestamp,
        DateTimeOffset timestamp)
    {
        Id = id;
        UserId = userId;
        TokenId = tokenId;
        DeviceName = deviceName;
        ClientName = clientName;
        Location = location;
        LastRefreshTimestamp = lastUsedTimestamp;
        Timestamp = timestamp;
    }

    public void UpdateTokenId()
    {
        LastRefreshTimestamp = DateTimeOffset.UtcNow;
        TokenId = Guid.NewGuid();
    }
}
