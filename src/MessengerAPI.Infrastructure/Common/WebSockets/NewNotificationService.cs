using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Schemas.Common;
using Microsoft.AspNetCore.SignalR;

namespace MessengerAPI.Infrastructure.Common.WebSockets;

public class NewNotificationService : INotificationService
{
    private readonly IHubContext<UpdatesHub> _hubContext;
    public NewNotificationService(IHubContext<UpdatesHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NewMessageReceived(List<Guid> recipientIds, MessageSchema message)
    {
        foreach (var userId in recipientIds)
        {
            await _hubContext.Clients.Groups(userId.ToString()).SendAsync("NewMessageReceived", message);
        }
    }

    public async Task MessageUpdated(List<Guid> recipientIds, MessageSchema message)
    {
        foreach (var userId in recipientIds)
        {
            await _hubContext.Clients.Groups(userId.ToString()).SendAsync("MessageUpdated", message);
        }
    }

    public async Task NewChannelCreated(List<Guid> recipientIds, ChannelSchema channel)
    {
        foreach (var userId in recipientIds)
        {
            await _hubContext.Clients.Groups(userId.ToString()).SendAsync("NewChannelCreated", channel);
        }
    }
}
