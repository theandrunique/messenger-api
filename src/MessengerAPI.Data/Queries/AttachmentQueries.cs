using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Queries;

internal class AttachmentQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelIdAndMessageIds;
    private readonly PreparedStatement _removeByChannelIdAndMessageId;

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
        _selectByChannelIdAndMessageIds = session.Prepare("SELECT * FROM attachments WHERE channelid = ? AND messageid IN ?");

        _removeByChannelIdAndMessageId = session.Prepare("DELETE FROM attachments WHERE channelid = ? AND messageid = ?");
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

    public BoundStatement RemoveByChannelIdAndMessageId(long channelId, long messageId)
    {
        return _removeByChannelIdAndMessageId.Bind(channelId, messageId);
    }
}
