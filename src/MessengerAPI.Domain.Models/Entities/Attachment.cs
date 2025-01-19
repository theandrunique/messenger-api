namespace MessengerAPI.Domain.Models.Entities;

public class Attachment
{
    public long Id { get; private set; }
    public long MessageId { get; private set; }
    public long ChannelId { get; private set; }
    public string Filename { get; private set; }
    public string UploadedFilename { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; }
    public string PreSignedUrl { get; private set; }
    public DateTimeOffset PreSignedUrlExpiresAt { get; private set; }
    public string? Placeholder { get; private set; }
    public float? DurationSecs { get; private set; }
    public string? Waveform { get; private set; }
    public bool IsSpoiler { get; private set; }

    public Attachment(
        long id,
        long channelId,
        string filename,
        string uploadedFilename,
        string contentType,
        long size,
        string preSignedUrl,
        DateTimeOffset preSignedUrlExpiresAt)
    {
        Id = id;
        ChannelId = channelId;
        Filename = filename;
        UploadedFilename = uploadedFilename;
        ContentType = contentType;
        Size = size;
        PreSignedUrl = preSignedUrl;
        PreSignedUrlExpiresAt = preSignedUrlExpiresAt;
    }

    public Attachment() { }

    public void UpdatePreSignedUrl(string preSignedUrl, DateTimeOffset preSignedUrlExpiresAt)
    {
        PreSignedUrl = preSignedUrl;
        PreSignedUrlExpiresAt = preSignedUrlExpiresAt;
    }
}

