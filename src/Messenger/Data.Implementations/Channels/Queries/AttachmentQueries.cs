using Cassandra;
using Messenger.Domain.Entities;

namespace Messenger.Data.Implementations.Channels.Queries;

public class AttachmentQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _selectByChannelIdAndMessageIds;
    private readonly PreparedStatement _selectByChannelIdAndId;
    private readonly PreparedStatement _selectByChannelId;
    private readonly PreparedStatement _removeByChannelIdAndMessageId;
    private readonly PreparedStatement _updatePreSignedUrl;

    public AttachmentQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO attachments_by_messageid (
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
                waveform,
                timestamp
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        """);
        _selectByChannelIdAndMessageIds = session.Prepare("SELECT * FROM attachments_by_messageid WHERE channelid = ? AND messageid IN ?");

        _selectByChannelIdAndId = session.Prepare("SELECT * FROM attachments_by_id WHERE channelid = ? AND id = ?");

        _selectByChannelId = session.Prepare("SELECT * FROM attachments_by_id WHERE channelid = ? AND id < ? LIMIT ?");

        _removeByChannelIdAndMessageId = session.Prepare("DELETE FROM attachments_by_messageid WHERE channelid = ? AND messageid = ?");

        _updatePreSignedUrl = session.Prepare("UPDATE attachments_by_messageid SET presignedurl = ?, presignedurlexpirestimestamp = ? WHERE channelid = ? AND messageid = ? AND id = ?");
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
            attachment.Waveform,
            attachment.Timestamp);
    }

    public BoundStatement SelectByChannelIdAndId(long channelId, long attachmentId)
    {
        return _selectByChannelIdAndId.Bind(channelId, attachmentId);
    }

    public BoundStatement SelectByChannelIdInMessageIds(long channelId, IEnumerable<long> messageIds)
    {
        return _selectByChannelIdAndMessageIds.Bind(channelId, messageIds);
    }

    public BoundStatement SelectByChannelId(long channelId, long before, int limit)
    {
        return _selectByChannelId.Bind(channelId, before, limit);
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
        return _updatePreSignedUrl.Bind(preSignedUrl, preSignedUrlExpiresTimestamp, channelId, messageId, attachmentId);
    }
}
