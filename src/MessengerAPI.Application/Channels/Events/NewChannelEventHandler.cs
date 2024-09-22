using System.Text.Json;
using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Schemas.Notifications;
using MessengerAPI.Domain.ChannelAggregate.Events;

namespace MessengerAPI.Application.Channels.Events;

public class NewChannelEventHandler : INotificationHandler<NewChannelCreated>
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public NewChannelEventHandler(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }


    public async Task Handle(NewChannelCreated notification, CancellationToken cancellationToken)
    {
        var mapped = _mapper.Map<NewChannelNotificationSchema>(notification);
        var jsonMessage = JsonSerializer.Serialize(mapped, JsonOptions.Default);

        var recipientIds = notification.NewChannel.Members.Select(m => m.Id).ToList();

        await _notificationService.Notify(recipientIds, jsonMessage);
    }
}
