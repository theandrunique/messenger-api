using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Channels.Events;

public class ChannelCreateEventHandler : INotificationHandler<ChannelCreate>
{
    private readonly INotificationService _notificationService;
    private readonly IMapper _mapper;

    public ChannelCreateEventHandler(INotificationService notificationService, IMapper mapper)
    {
        _notificationService = notificationService;
        _mapper = mapper;
    }

    public async Task Handle(ChannelCreate notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
