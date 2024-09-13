using System.Net.WebSockets;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces;

public interface IWebSocketService
{
    Task ConnectionAdded(UserId userId, WebSocket webSocket);
    Task ConnectionClosed(UserId userId);
}
