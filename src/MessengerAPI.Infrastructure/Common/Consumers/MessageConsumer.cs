using AutoMapper;
using MassTransit;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;

namespace MessengerAPI.Infrastructure.Common.Consumers;

public class MessageConsumer : IConsumer<Message>
{
    private readonly INotificationService _notificationService;
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public MessageConsumer(
        INotificationService notificationService,
        IChannelRepository channelRepository,
        IMapper mapper,
        IPublishEndpoint publisher)
    {
        _notificationService = notificationService;
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<Message> context)
    {
        List<Guid> recipientIds = await _channelRepository.GetMemberIdsFromChannelByIdOrNullAsync(
            context.Message.ChannelId,
            context.CancellationToken);

        await _notificationService.NewMessageReceived(recipientIds, _mapper.Map<MessageSchema>(context.Message));
    }
}
