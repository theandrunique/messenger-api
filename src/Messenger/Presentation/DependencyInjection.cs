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
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;

namespace Messenger.Infrastructure.Common.FileStorage;

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

    public static IServiceCollection AddMonitoring(this IServiceCollection services, ConfigurationManager _)
    {
        const string ServiceName = "app";
        const string OtlpExporterEndpoint = "http://otel-collector:4317";

        services.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(ServiceName))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.DisplayName = $"{response.HttpContext.Request.Method} {response.HttpContext.Request.Path} {response.StatusCode}";
                        };

                        options.Filter = (httpContext) =>
                        {
                            if (httpContext.Request.Method == "OPTIONS") return false;
                            if (httpContext.Request.Path.Value?.StartsWith("/swagger") == true) return false;
                            return true;
                        };

                    })
                    .AddInfrastructureInstrumentation()
                    .AddDataServicesInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(OtlpExporterEndpoint);
                    });
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddInfrastructureInstrumentation()
                    .AddDataServicesInstrumentation()
                    .AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(OtlpExporterEndpoint);
                    });
            });

        return services;
    }

    private static IServiceCollection AddCorsPolicy(this IServiceCollection services, ConfigurationManager config)
    {
        var corsOptions = new CorsPolicy();
        config.Bind(nameof(CorsPolicy), corsOptions);

        services.AddCors(options =>
            {
                options.AddPolicy(MessengerConstants.Cors.PolicyName, builder =>
                {
                    builder.WithOrigins(corsOptions.AllowedOrigins.ToArray())
                        .WithHeaders(corsOptions.AllowedHeaders.ToArray())
                        .WithMethods(corsOptions.AllowedMethods.ToArray())
                        .WithExposedHeaders(corsOptions.ExposedHeaders.ToArray())
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
