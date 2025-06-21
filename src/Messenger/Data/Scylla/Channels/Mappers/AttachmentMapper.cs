using Cassandra;
using Messenger.Domain.Messages;

namespace Messenger.Data.Scylla.Channels.Mappers;

public class AttachmentMapper
{
    public static Attachment Map(Row row)
    {
        return new Attachment(
            id: row.GetValue<long>("attachment_id"),
            messageId: row.GetValue<long>("message_id"),
            channelId: row.GetValue<long>("channel_id"),
            filename: row.GetValue<string>("filename"),
            contentType: row.GetValue<string>("content_type"),
            size: row.GetValue<long>("size"),
            preSignedUrl: row.GetValue<string>("presigned_url"),
            preSignedUrlExpiresTimestamp: row.GetValue<DateTimeOffset>("presigned_url_expires_timestamp"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"));
    }
}
