using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Contracts;

public static class DependencyInjection
{
    public static IServiceCollection AddContractsMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}