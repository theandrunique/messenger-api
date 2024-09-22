using System.Text.Json;
using System.Text.Json.Serialization;
using MessengerAPI.Application;
using MessengerAPI.Infrastructure;
using MessengerAPI.Infrastructure.Common.FileStorage;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("Frontend", builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .SetIsOriginAllowed((host) => true)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    })
    .AddProblemDetails()
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddPresentation();

var app = builder.Build();

app.UseCors("Frontend");

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
