using System.Net.WebSockets;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Interfaces;

public interface IWebSocketService
{
    /// <summary>
    /// Add new connection
    /// </summary>
    /// <param name="userId">Id of connected user<see cref="UserId"/></param>
    /// <param name="webSocket"><see cref="WebSocket"/></param>
    Task AddConnectionAsync(UserId userId, WebSocket webSocket);
    /// <summary>
    /// Remove connection by user id    
    /// </summary>
    /// <param name="userId">Id of connected user<see cref="UserId"/></param>
    Task RemoveConnectionAsync(UserId userId);
}
