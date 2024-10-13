using System.Reflection;
using FluentValidation;
using MessengerAPI.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace MessengerAPI.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Add application layer dependencies
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

        return services;
    }
}
