using Cassandra;
using Messenger.Data.Implementations.Channels.Dto;

namespace Messenger.Data.Implementations.Channels.Mappers;

public class MessageInfoMapper
{
    public static MessageInfoDto Map(Row row)
    {
        return new MessageInfoDto()
        {
            Id = row.GetValue<long>("id"),
            AuthorId = row.GetValue<long>("authorid"),
            TargetUserId = row.GetValue<long?>("targetuserid"),
            Content = row.GetValue<string>("content"),
            Timestamp = row.GetValue<DateTimeOffset>("timestamp"),
            EditedTimestamp = row.GetValue<DateTimeOffset?>("editedtimestamp"),
            AttachmentsCount = row.GetValue<int>("attachmentscount"),
            Type = row.GetValue<int>("type"),
            Metadata = row.GetValue<string?>("metadata"),
        };
    }
}
