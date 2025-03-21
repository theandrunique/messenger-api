namespace MessengerAPI.Domain.Entities;

public class Attachment
{
    public long Id { get; private set; }
    public long? MessageId { get; private set; }
    public long ChannelId { get; private set; }
    public string Filename { get; private set; }
    public string UploadFilename { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; }
    public string PreSignedUrl { get; private set; }
    public DateTimeOffset PreSignedUrlExpiresTimestamp { get; private set; }
    public string? Placeholder { get; private set; }
    public float? DurationSecs { get; private set; }
    public string? Waveform { get; private set; }
    public bool IsSpoiler { get; private set; }

    public Attachment(
        long id,
        long? messageId,
        long channelId,
        string filename,
        string uploadFilename,
        string contentType,
        long size,
        string preSignedUrl,
        DateTimeOffset preSignedUrlExpiresTimestamp)
    {
        Id = id;
        MessageId = messageId;
        ChannelId = channelId;
        Filename = filename;
        UploadFilename = uploadFilename;
        ContentType = contentType;
        Size = size;
        PreSignedUrl = preSignedUrl;
        PreSignedUrlExpiresTimestamp = preSignedUrlExpiresTimestamp;
    }

    public bool IsNeedUpdateUrl()
        => PreSignedUrlExpiresTimestamp < DateTimeOffset.UtcNow - TimeSpan.FromDays(1);

    public void UpdatePreSignedUrl(string preSignedUrl, DateTimeOffset preSignedUrlExpiresTimestamp)
    {
        PreSignedUrl = preSignedUrl;
        PreSignedUrlExpiresTimestamp = preSignedUrlExpiresTimestamp;
    }

    public void SetMessageId(long messageId)
    {
        if (MessageId != null)
        {
            throw new Exception("MessageId already set.");
        }

        MessageId = messageId;
    }
}
