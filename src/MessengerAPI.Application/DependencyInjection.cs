using System.Net.Mail;
using System.Reflection;
using FluentValidation;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Behaviors;
using MessengerAPI.Application.Common.Captcha;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Core;
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
        services.AddUserServices();
        services.AddChannelServices(config);

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
        services.AddValidatedOptions<AuthOptions>(config.GetSection(nameof(AuthOptions)));
        services.AddScoped<AuthService>();

        return services;
    }

    public static IServiceCollection AddChannelServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddScoped<AttachmentService>();
        services.Configure<AttachmentsOptions>(config.GetSection(nameof(AttachmentsOptions)));

        return services;
    }

    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<VerificationCodeService>();

        return services;
    }
}
