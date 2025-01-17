namespace MessengerAPI.Application.Channels.Common;

public record UploadFileData
{
    public string Filename { get; init; }
    public long Size { get; init; }
}
