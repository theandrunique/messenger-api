using Messenger.Application;
using Messenger.Core;
using Messenger.Gateway;
using Messenger.Infrastructure;
using Messenger.Infrastructure.Common.FileStorage;
using Messenger.Data.Implementations;
using Messenger.Presentation.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCoreServices()
    .AddGatewayServices()
    .AddDataServices(builder.Configuration)
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);

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
