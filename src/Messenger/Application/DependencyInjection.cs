using System.Reflection;
using FluentValidation;
using Messenger.Application.Auth.Common;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common;
using Messenger.Application.Common.Behaviors;
using Messenger.Application.Common.Captcha;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users.Common;
using Messenger.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Messenger.Application;

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
        services.AddUserServices(config);
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
        services.Configure<AttachmentOptions>(config.GetSection(nameof(AttachmentOptions)));
        services.AddScoped<ChannelImageService>();
        services.Configure<ChannelImageServiceOptions>(config.GetSection(nameof(ChannelImageServiceOptions)));

        return services;
    }

    public static IServiceCollection AddUserServices(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<VerificationCodeService>();
        services.AddScoped<AvatarService>();
        services.Configure<AvatarServiceOptions>(config.GetSection(nameof(AvatarServiceOptions)));

        return services;
    }
}
