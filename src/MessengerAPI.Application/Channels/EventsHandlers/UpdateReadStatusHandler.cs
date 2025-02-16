using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Channels.EventsHandlers;

public class UpdateReadStatusHandler : INotificationHandler<MessageCreateDomainEvent>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;

    public UpdateReadStatusHandler(IChannelRepository channelRepository, IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
    }

    public async Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _channelRepository.UpdateReadAt(_clientInfo.UserId, @event.Channel.Id, @event.Message.Id);
    }
}
