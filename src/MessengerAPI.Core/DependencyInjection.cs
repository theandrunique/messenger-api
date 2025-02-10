using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IIdGenerator, IdGenerator>();

        return services;
    }

    public static void AddValidatedOptions<T>(
        this IServiceCollection services,
        IConfigurationSection section)
        where T : class
    {
        services.AddOptions<T>()
            .Bind(section)
            .ValidateOnStart();
    }
}
