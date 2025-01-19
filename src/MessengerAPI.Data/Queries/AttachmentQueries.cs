using Cassandra;
using MessengerAPI.Domain.Models.Entities;

namespace MessengerAPI.Data.Queries;

internal class AttachmentQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelIdAndMessageIds;

    public AttachmentQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO attachments (
                channelid,
                messageid,
                id,
                contenttype,
                durationsecs,
                filename,
                isspoiler,
                placeholder,
                presignedurl,
                presignedurlexpirestimestamp,
                size,
                uploadfilename,
                waveform
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """);
        _selectByChannelIdAndMessageIds = session.Prepare(
            "SELECT * FROM attachments WHERE channelid = ? AND messageid IN ?"
        );
    }

    public BoundStatement Insert(Attachment attachment)
    {
        if (attachment.MessageId == null)
        {
            throw new ArgumentException("MessageId of attachment has to set before inserting.");
        }

        return _insert.Bind(
            attachment.ChannelId,
            attachment.MessageId,
            attachment.Id,
            attachment.ContentType,
            attachment.DurationSecs,
            attachment.Filename,
            attachment.IsSpoiler,
            attachment.Placeholder,
            attachment.PreSignedUrl,
            attachment.PreSignedUrlExpiresTimestamp,
            attachment.Size,
            attachment.UploadFilename,
            attachment.Waveform
        );
    }

    public BoundStatement SelectByChannelIdInMessageIds(long channelId, IEnumerable<long> messageIds)
    {
        return _selectByChannelIdAndMessageIds.Bind(channelId, messageIds);
    }
}
