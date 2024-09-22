using System.Text.Json;
using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Notifications;
using MessengerAPI.Domain.ChannelAggregate.Events;

namespace MessengerAPI.Application.Channels.Events;

public class MessageUpdatedEventHandler : INotificationHandler<MessageUpdated>
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;
    private readonly IChannelRepository _channelRepository;

    public MessageUpdatedEventHandler(IChannelRepository channelRepository, IMapper mapper, INotificationService notificationService)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _notificationService = notificationService;
    }

    public async Task Handle(MessageUpdated notification, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<MessageUpdatedNotificationSchema>(notification);
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
