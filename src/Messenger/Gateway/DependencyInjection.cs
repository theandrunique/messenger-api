using System.Reflection;
using Messenger.Gateway.Common;
using Messenger.Gateway.Serializers;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Gateway;

public static class DependencyInjection
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddScoped<IGatewayService, MessengerGatewayService>();
        services.AddSingleton<IEventSerializer, JsonEventSerializer>();
        services.AddScoped<ChannelRecipientsProvider>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
