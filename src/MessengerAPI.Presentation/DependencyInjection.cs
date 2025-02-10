using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using MessengerAPI.Core;
using MessengerAPI.Presentation.Common;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog;

namespace MessengerAPI.Infrastructure.Common.FileStorage;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddProblemDetails();
        services.AddSingleton<ProblemDetailsFactory, MessengerProblemDetailsFactory>();
        services.AddSerilog((services, options) =>
        {
            options.ReadFrom.Configuration(config);
        });

        services.AddCorsPolicy(config);
        services.AddControllersWithJsonOptions();
        services.AddSwagger();

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
