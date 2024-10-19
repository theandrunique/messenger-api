namespace MessengerAPI.Domain.ChannelAggregate.Entities;

public class Attachment
{
    public long Id { get; private set; }
    public long MessageId { get; private set;}
    public Guid ChannelId { get; private set; }
    public string Filename { get; private set; }
    public string UploadedFilename { get; private set; }
    public string ContentType { get; private set; }
    public long Size { get; private set; }
    public string PreSignedUrl { get; private set; }
    public DateTime PreSignedUrlExpiresAt { get; private set; }
    public string? Placeholder { get; private set; }
    public float? DurationSecs { get; private set; }
    public string? Waveform { get; private set; }
    public bool IsSpoiler { get; private set; }

    public static Attachment Create(
        Guid channelId,
        string filename,
        string uploadedFilename,
        string contentType,
        long size,
        string preSignedUrl,
        DateTime preSignedUrlExpiresAt)
    {
        var attachment = new Attachment
        {
            ChannelId = channelId,
            Filename = filename,
            UploadedFilename = uploadedFilename,
            ContentType = contentType,
            Size = size,
            PreSignedUrl = preSignedUrl,
            PreSignedUrlExpiresAt = preSignedUrlExpiresAt,
        };

        return attachment;
    }

    public void UpdatePreSignedUrl(string preSignedUrl, DateTime preSignedUrlExpiresAt)
    {
        PreSignedUrl = preSignedUrl;
        PreSignedUrlExpiresAt = preSignedUrlExpiresAt;
    }

    private Attachment() {}
}
