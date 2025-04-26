using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Contracts.Common;

/// <summary>
/// Session schema for response <see cref="Session"/>
/// </summary>
public class SessionSchema
{
    public string Id { get; private set; }
    public Guid TokenId { get; private set; }
    public string DeviceName { get; private set; }
    public string ClientName { get; private set; }
    public string Location { get; private set; }
    public DateTimeOffset LastUpdateTimestamp { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    private SessionSchema(Session session)
    {
        Id = session.Id.ToString();
        TokenId = session.TokenId;
        DeviceName = session.DeviceName;
        ClientName = session.ClientName;
        Location = session.Location;
        LastUpdateTimestamp = session.LastUsedTimestamp;
        Timestamp = session.Timestamp;
    }

    public static SessionSchema From(Session session) => new(session);
    public static List<SessionSchema> From(IEnumerable<Session> sessions) => sessions.Select(From).ToList();
}
