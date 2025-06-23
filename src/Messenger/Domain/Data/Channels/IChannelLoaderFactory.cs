namespace Messenger.Domain.Data.Channels;

public interface IChannelLoaderFactory
{
    IChannelLoader CreateLoader();
}
