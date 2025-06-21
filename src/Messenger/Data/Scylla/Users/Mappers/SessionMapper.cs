using Cassandra;
using Session = Messenger.Domain.Auth.Session;

namespace Messenger.Data.Scylla.Users.Mappers;

public static class SessionMapper
{
    internal static Session Map(Row row)
    {
        return new Session(
            id: row.GetValue<long>("session_id"),
            userId: row.GetValue<long>("user_id"),
            tokenId: row.GetValue<Guid>("token_id"),
            deviceName: row.GetValue<string>("device_name"),
            clientName: row.GetValue<string>("client_name"),
            location: row.GetValue<string>("location"),
            lastUsedTimestamp: row.GetValue<DateTimeOffset>("last_used_timestamp"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp")
        );
    }
}
