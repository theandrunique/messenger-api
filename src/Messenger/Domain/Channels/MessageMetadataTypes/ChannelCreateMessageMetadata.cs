namespace Messenger.Domain.Channels.MessageMetadataTypes;

public struct ChannelCreateMessageMetadata : IMessageMetadata
{
    public string ChannelName { get; init; }

    public ChannelCreateMessageMetadata(string channelName)
    {
        ChannelName = channelName;
    }
}
