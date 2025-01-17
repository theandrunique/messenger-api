using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IIdGenerator, IdGenerator>();

        return services;
    }
}
