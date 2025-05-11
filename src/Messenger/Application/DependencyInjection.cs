using System.Reflection;
using FluentValidation;
using Messenger.Application.Auth.Common;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common.Behaviors;
using Messenger.Application.Common.Captcha;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Users.Common;
using Messenger.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Messenger.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("en");

        services.AddCaptchaService();
        services.AddAuthServices();
        services.AddUserServices();
        services.AddChannelServices();

        return services;
    }

    private static IServiceCollection AddCaptchaService(this IServiceCollection services)
    {
        services.AddRefitClient<IHCaptchaApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value.HCaptchaOptions;
                client.BaseAddress = new Uri(options.ApiBaseUrl);
            });

        services.AddScoped<CaptchaService>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();

        return services;
    }

    public static IServiceCollection AddChannelServices(this IServiceCollection services)
    {
        services.AddScoped<AttachmentService>();
        services.AddScoped<ChannelImageService>();

        return services;
    }

    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<VerificationCodeService>();
        services.AddScoped<AvatarService>();

        return services;
    }
}
