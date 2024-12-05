using System.Reflection;
using FluentValidation;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Behaviors;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace MessengerAPI.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

        services.AddCaptchaService(config);

        services.Configure<SmtpOptions>(config.GetSection(nameof(SmtpOptions)));

        return services;
    }

    private static IServiceCollection AddCaptchaService(this IServiceCollection services, ConfigurationManager config)
    {
        var captchaOptions = new HCaptchaOptions();
        config.Bind(nameof(HCaptchaOptions), captchaOptions);
        services.AddSingleton(Options.Create(captchaOptions));

        services.AddRefitClient<IHCaptchaApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(captchaOptions.ApiBaseUrl);
            });

        services.AddScoped<CaptchaService>();

        return services;
    }
}
