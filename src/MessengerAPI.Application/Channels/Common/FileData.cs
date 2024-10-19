namespace MessengerAPI.Application.Channels.Common;

public record FileData
{
    public string Filename { get; init; }
    public long Size { get; init; }
}
