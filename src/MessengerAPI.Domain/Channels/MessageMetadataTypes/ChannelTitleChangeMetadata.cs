namespace MessengerAPI.Domain.Channels.MessageMetadataTypes;

public class ChannelTitleChangeMetadata : IMessageMetadata
{
    public string NewTitle { get; set; }

    public ChannelTitleChangeMetadata(string newTitle) => NewTitle = newTitle;
}
