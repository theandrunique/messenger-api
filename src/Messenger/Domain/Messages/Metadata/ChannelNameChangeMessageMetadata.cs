namespace Messenger.Domain.Messages.Metadata;

public struct ChannelNameChangeMessageMetadata : IMessageMetadata
{
    public string NewName { get; set; }

    public ChannelNameChangeMessageMetadata(string newName) => NewName = newName;
}
