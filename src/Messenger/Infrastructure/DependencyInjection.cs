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
using Messenger.Infrastructure.Users;
using Messenger.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using StackExchange.Redis;

namespace Messenger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISmtpClient, SmtpClient>();

        services.AddHttpContextAccessor();

        services.AddAuthServices();
        services.AddS3Services();
        services.AddRedisClient();
        services.AddElasticsearch();
        services.AddSingleton<IIdGenerator, IdGenerator>();

        services.AddScoped<IImageProcessor, ImageProcessor>();

        return services;
    }

    private static IServiceCollection AddS3Services(this IServiceCollection services)
    {
        AWSConfigsS3.UseSignatureVersion4 = true;

        services.AddSingleton<IAmazonS3>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value.S3Options;
            return new AmazonS3Client(
                credentials: new BasicAWSCredentials(options.AccessKey, options.SecretKey),
                clientConfig: new AmazonS3Config()
                {
                    ServiceURL = options.ServiceUrl,
                    ForcePathStyle = options.ForcePathStyle,
                });
        });
        services.AddScoped<IS3Service, S3Service>();

        return services;
    }

    private static IServiceCollection AddRedisClient(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value.RedisOptions;
            return ConnectionMultiplexer.Connect(options.ConnectionString);
        });

        return services;
    }

    private static IServiceCollection AddElasticsearch(this IServiceCollection services)
    {
        services.AddSingleton<ElasticsearchClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ApplicationOptions>>().Value.ElasticSearchOptions;

            var settings = new ElasticsearchClientSettings(new Uri(options.Uri))
                .DefaultMappingFor<UserIndexModel>(i => i.IndexName("users"))
                .EnableDebugMode();

            return new ElasticsearchClient(settings);
        });

        services.AddHostedService<ElasticsearchIndexInitializationService>();
        services.AddScoped<IUserSearchService, UserSearchService>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services)
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
