using Messenger.Domain.Channels;
using Messenger.Domain.Messages;

namespace Messenger.Contracts.Common;

public record AttachmentSchema
{
    public string Id { get; init; }
    public string Filename { get; init; }
    public string ContentType { get; init; }
    public long Size { get; init; }
    public string Url { get; init; }
    public DateTimeOffset Timestamp { get; init; }

    private AttachmentSchema(Attachment attachment)
    {
        Id = attachment.Id.ToString();
        Filename = attachment.Filename;
        ContentType = attachment.ContentType;
        Size = attachment.Size;
        Url = attachment.PreSignedUrl;
        Timestamp = attachment.Timestamp;
    }

    public static AttachmentSchema From(Attachment attachment) => new AttachmentSchema(attachment);

    public static List<AttachmentSchema> From(IEnumerable<Attachment> attachments) => attachments.Select(From).ToList();
}
