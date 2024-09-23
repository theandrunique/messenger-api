using System.Text.Json;
using MessengerAPI.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class SubscriberService
{
    private readonly string _serverId;
    private readonly ISubscriber _subscriber;
    private readonly IServiceScopeFactory _scopeFactory;

    public SubscriberService(IConnectionMultiplexer connectionMultiplexer, IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;

        _subscriber = connectionMultiplexer.GetSubscriber();

        _subscriber.Subscribe(_serverId, async (c, m) => await HandleNotifications(m));
    }

    private async Task HandleNotifications(string message)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var serializedMessage = JsonSerializer.Deserialize<NotificationMessage>(message);
            if (serializedMessage == null) return;

            await notificationService.NotifyAsync(serializedMessage.RecipientIds, serializedMessage.JsonData);
        }
    }
}
