namespace Messenger.Domain.Messages.Metadata;

public struct ChannelImageChangeMessageMetadata : IMessageMetadata
{
    public string NewImage { get; set; }

    public ChannelImageChangeMessageMetadata(string newImage) => NewImage = newImage;
}
