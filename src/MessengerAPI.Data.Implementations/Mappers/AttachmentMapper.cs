using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Implementations.Mappers;

public class AttachmentMapper
{
    public static Attachment Map(Row row)
    {
        return new Attachment(
            row.GetValue<long>("id"),
            row.GetValue<long>("messageid"),
            row.GetValue<long>("channelid"),
            row.GetValue<string>("filename"),
            row.GetValue<string>("contenttype"),
            row.GetValue<long>("size"),
            row.GetValue<string>("presignedurl"),
            row.GetValue<DateTimeOffset>("presignedurlexpirestimestamp"),
            row.GetValue<DateTimeOffset>("timestamp"));
    }
}
