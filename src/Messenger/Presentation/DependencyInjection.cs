using System.Text.Json;
using System.Text.Json.Serialization;
using Messenger.Data.Scylla;
using Messenger.Core;
using Messenger.Presentation.Common;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using OpenTelemetry.Metrics;
using Messenger.Infrastructure;
using Messenger.Options;
using Cassandra;

namespace Messenger.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddProblemDetails();
        services.AddSingleton<ProblemDetailsFactory, MessengerProblemDetailsFactory>();
        services.AddSerilog((services, options) =>
        {
            options.ReadFrom.Configuration(config);
        });

        services.AddCorsPolicy(config);
        services.AddControllersWithJsonOptions();
        services.AddSwagger();
        services.AddMonitoring(config);

        return services;
    }

    public static IServiceCollection AddMonitoring(this IServiceCollection services, ConfigurationManager config)
    {
        var enabled = config.GetValue<bool>(nameof(ApplicationOptions.MONITORING_ENABLED));
        if (!enabled) return services;

        services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddInfrastructureInstrumentation()
                    .AddDataServicesInstrumentation()
                    .AddOtlpExporter();
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddInfrastructureInstrumentation()
                    .AddDataServicesInstrumentation()
                    .AddOtlpExporter();
            });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, ConfigurationManager config)
    {
        var options = config.Get<CorsPolicy>();

        var corsOptions = new CorsPolicy();
        config.Bind(nameof(CorsPolicy), corsOptions);

        services.AddCors(options =>
            {
                options.AddPolicy(MessengerConstants.Cors.PolicyName, builder =>
                {
                    builder
                        .WithOrigins(corsOptions.AllowedOrigins.ToArray())
                        .WithHeaders(corsOptions.AllowedHeaders.ToArray())
                        .WithMethods(corsOptions.AllowedMethods.ToArray())
                        .WithExposedHeaders(corsOptions.ExposedHeaders.ToArray())
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                        .AllowCredentials();
                });
            });
        return services;
    }

    private static IServiceCollection AddControllersWithJsonOptions(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

        return services;
    }
}
