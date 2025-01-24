namespace MessengerAPI.Application.Channels.Common;

public record CreateAttachmentData
{
    public string Filename { get; init; }
    public long Size { get; init; }
}
