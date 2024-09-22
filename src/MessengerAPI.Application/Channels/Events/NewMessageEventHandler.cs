using System.Text.Json;
using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Notifications;
using MessengerAPI.Domain.ChannelAggregate.Events;

namespace MessengerAPI.Application.Channels.Events;

public class NewMessageEventHandler : INotificationHandler<NewMessageCreated>
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;
    private readonly IChannelRepository _channelRepository;

    public NewMessageEventHandler(INotificationService notificationService, IMapper mapper, IChannelRepository channelRepository)
    {
        _notificationService = notificationService;
        _mapper = mapper;
        _channelRepository = channelRepository;
    }

    /// <summary>
    /// Send a notification about new message
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Handle(NewMessageCreated notification, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<NewMessageNotificationSchema>(notification);
        var jsonMessage = JsonSerializer.Serialize(mapped, JsonOptions.Default);

        var channel = await _channelRepository.GetByIdAsync(notification.NewMessage.ChannelId, cancellationToken);
        if (channel is null)
        {
            return;
        }

        var recipientIds = channel.Members.Select(m => m.Id).ToList();

        await _notificationService.Notify(recipientIds, jsonMessage);
    }
}
