using MessengerAPI.Infrastructure.Common.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddPersistance();

        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));

        return services;
    }
}
