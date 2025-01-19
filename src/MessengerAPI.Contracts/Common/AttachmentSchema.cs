namespace MessengerAPI.Contracts.Common;

public record AttachmentSchema
{
    public string Id { get; init; }
    public string Filename { get; init; }
    public string ContentType { get; init; }
    public long Size { get; init; }
    public string Url { get; init; }
    public string? Placeholder { get; private set; }
    public float? DurationSecs { get; private set; }
    public string? Waveform { get; private set; }
    public bool IsSpoiler { get; private set; }
}
