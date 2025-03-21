using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Queries;

internal class AttachmentQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelIdAndMessageIds;
    private readonly PreparedStatement _selectByChannelIdAndId;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _removeByChannelIdAndMessageId;
    private readonly PreparedStatement _updatePreSignedUrls;

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

        _selectByChannelIdAndId = session.Prepare("SELECT * FROM attachments WHERE channelid = ? AND id = ?");

        _selectByChannelId = session.Prepare("SELECT * FROM attachments WHERE channelid = ? AND messageid < ? LIMIT ?");

        _removeByChannelIdAndMessageId = session.Prepare("DELETE FROM attachments WHERE channelid = ? AND messageid = ?");

        _updatePreSignedUrls = session.Prepare("UPDATE attachments SET presignedurl = ?, presignedurlexpirestimestamp = ? WHERE channelid = ? AND messageid = ? AND id = ?");
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

    public BoundStatement SelectByChannelIdAndId(long channelId, long attachmentId)
    {
        return _selectByChannelIdAndId.Bind(channelId, attachmentId);
    }

    public BoundStatement SelectByChannelIdInMessageIds(long channelId, IEnumerable<long> messageIds)
    {
        return _selectByChannelIdAndMessageIds.Bind(channelId, messageIds);
    }

    public BoundStatement SelectByChannelId(long channelId, long beforeMessageId, int limit)
    {
        return _selectByChannelId.Bind(channelId, beforeMessageId, limit);
    }

    public BoundStatement RemoveByChannelIdAndMessageId(long channelId, long messageId)
    {
        return _removeByChannelIdAndMessageId.Bind(channelId, messageId);
    }

    public BoundStatement UpdatePreSignedUrl(
        long channelId,
        long messageId,
        long attachmentId,
        string preSignedUrl,
        DateTimeOffset preSignedUrlExpiresTimestamp)
    {
        return _updatePreSignedUrls.Bind(preSignedUrl, preSignedUrlExpiresTimestamp, channelId, messageId, attachmentId);
    }
}
