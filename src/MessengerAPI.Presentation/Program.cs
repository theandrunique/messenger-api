using MessengerAPI.Application;
using MessengerAPI.Contracts;
using MessengerAPI.Infrastructure;
using MessengerAPI.Infrastructure.Common.FileStorage;
using MessengerAPI.Infrastructure.Common.WebSockets;
using MessengerAPI.Presentation.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddContractsMappings()
    .AddPresentation(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();


var app = builder.Build();

app.UseCors(CorsConstants.CorsPolicyName);

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();

app.UseWebSockets();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<UpdatesHub>("/ws");

app.Run();
