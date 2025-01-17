namespace MessengerAPI.Infrastructure.Common.WebSockets;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MessengerAPI.Infrastructure.Auth;

[Authorize]
public class UpdatesHub : Hub
{
    private async void AddConnection(long userId, string connectionId)
    {
        await Groups.AddToGroupAsync(connectionId, userId.ToString());
    }

    private async void RemoveConnection(long userId, string connectionId)
    {
        await Groups.RemoveFromGroupAsync(connectionId, userId.ToString());
    }

    public override async Task OnConnectedAsync()
    {
        AddConnection(Context.User.GetUserId(), Context.ConnectionId);

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        RemoveConnection(Context.User.GetUserId(), Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
}
