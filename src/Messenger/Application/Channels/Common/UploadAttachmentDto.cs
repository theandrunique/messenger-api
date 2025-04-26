namespace Messenger.Application.Channels.Common;

public record UploadAttachmentDto
{
    public string? Id { get; init; }
    public string Filename { get; init; }
    public long FileSize { get; init; }
}
