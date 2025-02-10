using MessengerAPI.Application;
using MessengerAPI.Contracts;
using MessengerAPI.Core;
using MessengerAPI.Data;
using MessengerAPI.Gateway;
using MessengerAPI.Infrastructure;
using MessengerAPI.Infrastructure.Common.FileStorage;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCoreServices()
    .AddGatewayServices()
    .AddContractsMappings()
    .AddDataServices(builder.Configuration)
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication(builder.Configuration);


var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseCors(MessengerConstants.Cors.PolicyName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
