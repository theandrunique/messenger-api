using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Auth;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Infrastructure.Auth;
using MessengerAPI.Infrastructure.Auth.SigningKeys;
using MessengerAPI.Infrastructure.Common;
using MessengerAPI.Infrastructure.Common.FileStorage;
using MessengerAPI.Infrastructure.Common.Persistance;
using MessengerAPI.Infrastructure.Common.WebSockets;
using MessengerAPI.Infrastructure.Persistance;
using MessengerAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        services.AddAuth(config);
        services.AddPersistance(config);

        services.AddSingleton<IHashHelper, BCryptHelper>();
        services.AddScoped<IClientInfoProvider, ClientInfoProvider>();
        services.AddScoped<IJweHelper, JweHelper>();
        services.AddScoped<IRevokedTokenStore, RevokedTokenStore>();

        services.Configure<FileStorageSettings>(config.GetSection(nameof(FileStorageSettings)));
        services.AddSingleton<IFileStorageSettings>(sp => sp.GetRequiredService<IOptions<FileStorageSettings>>().Value);
        services.AddScoped<IFileStorage, FileStorage>();

        services.AddRedis(config);

        services.AddWebSockets();

        return services;
    }

    public static IServiceCollection AddPersistance(this IServiceCollection services, ConfigurationManager config)
    {
        var postgresSettings = new PostgresSettings();
        config.Bind(nameof(PostgresSettings), postgresSettings);
        services.AddSingleton(Options.Create(postgresSettings));

        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(postgresSettings.ConnectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFileRepository, fileRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, ConfigurationManager config)
    {
        var redisSettings = new RedisSettings();
        config.Bind(nameof(RedisSettings), redisSettings);

        services.AddSingleton(Options.Create(redisSettings));

        var redis = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(sp => redis);
        return services;
    }

    public static IServiceCollection AddWebSockets(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IWebSocketService, NotificationService>();
        services.AddScoped<ConnectionRepository>();
        services.AddSingleton<SubscriberService>();

        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        var jwtSettings = new JwtSettings();
        config.Bind(nameof(JwtSettings), jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtSettings>(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

        services.AddSingleton<IJwtTokenGenerator, JwtTokenHelper>();

        services.AddSingleton<KeyManagementService>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
                options.MapInboundClaims = false;
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        Dictionary<string, string>? claims = context.Principal?.Claims.ToDictionary(c => c.Type, c => c.Value);
                        if (claims == null)
                        {
                            context.Fail("Invalid token.");
                            return;
                        }

                        if (!claims.ContainsKey(JwtRegisteredClaimNames.Sub))
                        {
                            context.Fail("Invalid token.");
                            return;
                        }
                        if (!claims.ContainsKey(JwtRegisteredClaimNames.Jti))
                        {
                            context.Fail("Invalid token.");
                            return;
                        }

                        var tokenId = claims[JwtRegisteredClaimNames.Jti];

                        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IRevokedTokenStore>();
                        if (!await cacheService.IsTokenValidAsync(tokenId))
                        {
                            context.Fail("Token has been revoked.");
                        }
                    }
                };
            });
        return services;
    }
}
