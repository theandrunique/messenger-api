using Messenger.Domain.Data.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Data.Scylla.Channels;

public class ChannelLoaderFactory : IChannelLoaderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ChannelLoaderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IChannelLoader CreateLoader()
    {
        return _serviceProvider.GetRequiredService<IChannelLoader>();
    }
}
