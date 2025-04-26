using System.Reflection;
using MessengerAPI.Gateway.Serializers;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Gateway;

public static class DependencyInjection
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddScoped<IGatewayService, MessengerGatewayService>();
        services.AddSingleton<IEventSerializer, JsonEventSerializer>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        return services;
    }
}
