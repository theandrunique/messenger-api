using Cassandra;
using Messenger.Domain.Channels;

namespace Messenger.Data.Scylla.Messages.Mappers;

public class MessageAckMapper
{
    public static MessageAck Map(Row row)
    {
        return new MessageAck(
            channelId: row.GetValue<long>("channelid"),
            userId: row.GetValue<long>("userid"),
            lastReadMessageId: row.GetValue<long>("lastreadmessageid"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"));
    }
}
