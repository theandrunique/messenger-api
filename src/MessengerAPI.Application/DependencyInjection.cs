using System.Reflection;
using FluentValidation;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Behaviors;
using MessengerAPI.Application.Common.Captcha;
using MessengerAPI.Application.Common.Interfaces;
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
        services.AddAuthServices(config);
        services.AddChannelServices();

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

    private static IServiceCollection AddAuthServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.Configure<AuthOptions>(config.GetSection(nameof(AuthOptions)));
        services.AddScoped<AuthService>();

        return services;
    }

    public static IServiceCollection AddChannelServices(this IServiceCollection services)
    {
        services.AddScoped<AttachmentService>();

        return services;
    }
}
