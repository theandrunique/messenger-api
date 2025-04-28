namespace Messenger.Domain.Channels.MessageMetadataTypes;

public struct ChannelNameChangeMessageMetadata : IMessageMetadata
{
    public string NewName { get; set; }

    public ChannelNameChangeMessageMetadata(string newName) => NewName = newName;
}
