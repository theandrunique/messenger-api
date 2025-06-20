namespace Messenger.Domain.Messages.Metadata;

public struct ChannelCreateMessageMetadata : IMessageMetadata
{
    public string ChannelName { get; init; }

    public ChannelCreateMessageMetadata(string channelName)
    {
        ChannelName = channelName;
    }
}
