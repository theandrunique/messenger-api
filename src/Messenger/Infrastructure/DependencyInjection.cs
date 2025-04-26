using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Elastic.Clients.Elasticsearch;
using Messenger.Application.Auth.Common.Interfaces;
using Messenger.Application.Common.Interfaces;
using Messenger.Application.Common.Interfaces.S3;
using Messenger.Application.Users.Common;
using Messenger.Core;
using Messenger.Domain.Users;
using Messenger.Infrastructure.Auth;
using Messenger.Infrastructure.Common;
using Messenger.Infrastructure.Common.Files;
using Messenger.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using StackExchange.Redis;

namespace Messenger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager config)
    {
        services.AddScoped<ISmtpClient, SmtpClient>();

        services.AddHttpContextAccessor();

        services.AddAuthServices();
        services.AddS3Services(config);
        services.AddRedis(config);
        services.AddElasticsearch();
        services.AddSingleton<IIdGenerator, IdGenerator>();

        services.AddScoped<IImageProcessor, ImageProcessor>();

        return services;
    }

    public static IServiceCollection AddS3Services(this IServiceCollection services, IConfigurationManager config)
    {
        AWSConfigsS3.UseSignatureVersion4 = true;

        var options = new S3Options();
        config.Bind(nameof(S3Options), options);

        var s3Client = new AmazonS3Client(
            credentials: new BasicAWSCredentials(options.AccessKey, options.SecretKey),
            clientConfig: new AmazonS3Config()
            {
                ServiceURL = options.ServiceUrl,
            });

        services.AddSingleton<IAmazonS3, AmazonS3Client>(sp => s3Client);

        services.Configure<S3Options>(config.GetSection(nameof(S3Options)));
        services.AddScoped<IS3Service, S3Service>();

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, ConfigurationManager config)
    {
        var redisSettings = new RedisOptions();
        config.Bind(nameof(RedisOptions), redisSettings);

        services.AddSingleton(Options.Create(redisSettings));

        var redis = ConnectionMultiplexer.Connect(redisSettings.ConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(sp => redis);

        return services;
    }

    public static IServiceCollection AddElasticsearch(this IServiceCollection services)
    {
        var settings = new ElasticsearchClientSettings(new Uri("http://elasticsearch:9200"))
            .DefaultMappingFor<UserIndexModel>(i => i.IndexName("users"))
            .EnableDebugMode();

        var client = new ElasticsearchClient(settings);
        services.AddSingleton(client);
        services.AddHostedService<ElasticsearchIndexInitializationService>();
        services.AddScoped<IUserSearchService, UserSearchService>();

        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddSingleton<IHashHelper, BCryptHelper>();
        services.AddScoped<IClientInfoProvider, ClientInfoProvider>();
        services.AddScoped<IJweHelper, JweHelper>();
        services.AddSingleton<IJwtHelper, JwtHelper>();
        services.AddScoped<IRevokedTokenService, RevokedTokenService>();
        services.AddSingleton<IKeyManagementService, KeyManagementService>();
        services.AddScoped<ITotpHelper, TotpHelper>();

        services.ConfigureOptions<JwtBearerOptionsConfiguration>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        return services;
    }

    public static TracerProviderBuilder AddInfrastructureInstrumentation(this TracerProviderBuilder builder)
    {
        builder
            .AddHttpClientInstrumentation()
            .AddAWSInstrumentation()
            .AddElasticsearchClientInstrumentation(options =>
            {
                options.SetDbStatementForRequest = true;
            })
            .AddRedisInstrumentation(options =>
            {
                options.SetVerboseDatabaseStatements = true;
            });

        return builder;
    }

    public static MeterProviderBuilder AddInfrastructureInstrumentation(this MeterProviderBuilder builder)
    {
        builder
            .AddHttpClientInstrumentation()
            .AddAWSInstrumentation();

        return builder;
    }
}
