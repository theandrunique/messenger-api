using Messenger.Application;
using Messenger.Core;
using Messenger.Gateway;
using Messenger.Infrastructure;
using Messenger.Data.Scylla;
using Messenger.Presentation;
using Messenger.Presentation.Common;
using Serilog;
using Microsoft.Extensions.Options;
using Messenger.Options;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateBootstrapLogger();

    builder.Services
        .AddApplicationOptions(builder.Configuration)
        .AddCoreServices()
        .AddGatewayServices()
        .AddDataServices()
        .AddPresentation(builder.Configuration)
        .AddInfrastructure()
        .AddApplication();

    var app = builder.Build();

    var options = app.Services.GetRequiredService<IOptions<ApplicationOptions>>().Value;
    Log.Information("App config: {@Config}", options);

    app.UseCors(MessengerConstants.Cors.PolicyName);

    app.UseSerilogRequestLogging(options =>
    {
        options.IncludeQueryInRequestPath = true;
        options.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;
    });

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseExceptionHandler();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (OptionsValidationException ex)
{
    Log.Fatal(ex, "Invalid configuration: {@Failures}", ex.Failures);
    Environment.Exit(1);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
    Environment.Exit(1);
}
finally
{
    Log.CloseAndFlush();
}
