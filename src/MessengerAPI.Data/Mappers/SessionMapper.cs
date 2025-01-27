using Cassandra;
using Session = MessengerAPI.Domain.Entities.Session;

namespace MessengerAPI.Data.Mappers;

internal static class SessionMapper
{
    internal static Session Map(Row row)
    {
        return new Session(
            id: row.GetValue<long>("id"),
            userId: row.GetValue<long>("userid"),
            tokenId: row.GetValue<Guid>("tokenid"),
            deviceName: row.GetValue<string>("devicename"),
            clientName: row.GetValue<string>("clientname"),
            location: row.GetValue<string>("location"),
            lastUsedTimestamp: row.GetValue<DateTimeOffset>("lastusedtimestamp"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp")
        );
    }
}
