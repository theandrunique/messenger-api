namespace Messenger.Domain.Channels.MessageMetadataTypes;

public struct ChannelImageChangeMessageMetadata : IMessageMetadata
{
    public string NewImage { get; set; }

    public ChannelImageChangeMessageMetadata(string newImage) => NewImage = newImage;
}
