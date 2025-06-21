using Cassandra;
using Messenger.Data.Scylla.Channels.Dto;

namespace Messenger.Data.Scylla.Channels.Mappers;

public class MessageInfoMapper
{
    public static MessageInfoDto Map(Row row)
    {
        return new MessageInfoDto()
        {
            Id = row.GetValue<long>("id"),
            AuthorId = row.GetValue<long>("author_id"),
            TargetUserId = row.GetValue<long?>("target_user_id"),
            Content = row.GetValue<string>("content"),
            Timestamp = row.GetValue<DateTimeOffset>("timestamp"),
            EditedTimestamp = row.GetValue<DateTimeOffset?>("edited_timestamp"),
            AttachmentsCount = row.GetValue<int>("attachments_count"),
            Type = row.GetValue<int>("type"),
            Metadata = row.GetValue<string?>("metadata"),
        };
    }
}
