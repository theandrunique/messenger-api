using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using StackExchange.Redis;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class NotificationService : INotificationService, IWebSocketService
{
    private static readonly ConcurrentDictionary<UserId, WebSocket> _connections = new();
    private readonly ConnectionRepository _connectionRepository;
    private readonly string _serverId;
    private readonly IDatabase _redis;

    public NotificationService(
        ConnectionRepository connectionRepository,
        IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionRepository = connectionRepository;
        _serverId = Environment.MachineName;
        _redis = connectionMultiplexer.GetDatabase();
    }

    public async Task ConnectionAdded(UserId userId, WebSocket webSocket)
    {
        _connections[userId] = webSocket;
        await _connectionRepository.Add(userId, _serverId);
    }

    public async Task ConnectionClosed(UserId userId)
    {
        if (_connections.ContainsKey(userId))
        {
            _connections.Remove(userId, out _);
            await _connectionRepository.Remove(userId);
        }
    }

    public async Task Notify(NotificationMessage message)
    {
        var groups = new Dictionary<string, List<UserId>>();
        var currentServer = new HashSet<UserId>();

        foreach (var userId in message.RecipientIds)
        {
            if (_connections.ContainsKey(userId))
            {
                currentServer.Add(userId);
            }
            else
            {
                var serverId = await _connectionRepository.Get(userId);
                if (serverId != null)
                {
                    if (!groups.ContainsKey(serverId))
                    {
                        groups[serverId] = new List<UserId>();
                    }
                    groups[serverId].Add(userId);
                }
            }
        }

        foreach (string pipe in groups.Keys)
        {
            var notificationMessage = new NotificationMessage(groups[pipe], message.JsonData);
            string jsonMessage = JsonSerializer.Serialize(notificationMessage);
            await _redis.PublishAsync(pipe, jsonMessage);
        }
        foreach (UserId recipientId in currentServer)
        {
            await SendMessage(recipientId, message.JsonData);
        }
    }

    public async Task SendMessage(UserId recipientId, string jsonMessage)
    {
        if (_connections.ContainsKey(recipientId))
        {
            await SendMessageToWebSocket(recipientId, jsonMessage);
        }
        else
        {
            string? serverId = await _connectionRepository.Get(recipientId);
            // TODO: check that queue exists
            if (serverId != null)
            {
                await _redis.PublishAsync(serverId, jsonMessage);
            }
        }
    }

    private async Task SendMessageToWebSocket(UserId userId, string jsonMessage)
    {
        if (_connections.TryGetValue(userId, out var webSocket))
        {
            await webSocket.SendAsync(
                new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonMessage)),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }
}
