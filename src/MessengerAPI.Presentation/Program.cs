using MessengerAPI.Application;
using MessengerAPI.Core;
using MessengerAPI.Gateway;
using MessengerAPI.Infrastructure;
using MessengerAPI.Infrastructure.Common.FileStorage;
using MessengerAPI.Data.Implementations;
using Serilog;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Cassandra.OpenTelemetry;
using OpenTelemetry.Resources;
using MessengerAPI.Presentation.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCoreServices()
    .AddGatewayServices()
    .AddDataServices(builder.Configuration)
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService("messenger-api-ddd-app-1"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource(CassandraActivitySourceHelper.ActivitySourceName)
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://tempo:4317");
            });
    });

var app = builder.Build();

app.UseCors(MessengerConstants.Cors.PolicyName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggingEnrichmentMiddleware>();
app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
