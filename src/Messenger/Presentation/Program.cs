using Messenger.Application;
using Messenger.Core;
using Messenger.Gateway;
using Messenger.Infrastructure;
using Messenger.Data.Scylla;
using Messenger.Presentation;
using Messenger.Presentation.Common;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateBootstrapLogger();

    builder.Services
        .AddCoreServices()
        .AddGatewayServices()
        .AddDataServices(builder.Configuration)
        .AddPresentation(builder.Configuration)
        .AddInfrastructure(builder.Configuration)
        .AddApplication(builder.Configuration);

    var app = builder.Build();

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
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
