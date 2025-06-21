using Cassandra;
using Messenger.Domain.Channels;

namespace Messenger.Data.Scylla.Messages.Mappers;

public class MessageAckMapper
{
    public static MessageAck Map(Row row)
    {
        return new MessageAck(
            channelId: row.GetValue<long>("channel_id"),
            userId: row.GetValue<long>("user_id"),
            lastReadMessageId: row.GetValue<long>("last_read_message_id"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"));
    }
}
