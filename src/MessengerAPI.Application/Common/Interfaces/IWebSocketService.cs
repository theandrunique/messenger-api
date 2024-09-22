using System.Net.WebSockets;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces;

public interface IWebSocketService
{
    Task AddConnection(UserId userId, WebSocket webSocket);
    Task RemoveConnection(UserId userId);
}
