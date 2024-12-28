using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Models.Events;

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

    /// <summary>
    /// Send a notification about new channel to all channel members
    /// </summary>
    /// <param name="notification"><see cref="NewChannelCreated"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    public async Task Handle(NewChannelCreated notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var recipientIds = notification.NewChannel.Members.Select(m => m.Id).ToList();

        // await _notificationService.NewChannelCreated(recipientIds, _mapper.Map<ChannelSchema>(notification.NewChannel));
    }
}
