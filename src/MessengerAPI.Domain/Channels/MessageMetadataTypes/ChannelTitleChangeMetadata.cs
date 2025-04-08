namespace MessengerAPI.Domain.Channels.MessageMetadataTypes;

public class ChannelNameChangeMetadata : IMessageMetadata
{
    public string NewName { get; set; }

    public ChannelNameChangeMetadata(string newName) => NewName = newName;
}
