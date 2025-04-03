using MediatR;
using MessengerAPI.Core;
using MessengerAPI.Data.Interfaces.Channels;
using MessengerAPI.Domain.Channels.MessageMetadataTypes;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Events;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Channels.EventHandlers;

public class MetaMessagesCreator
    : INotificationHandler<ChannelTitleUpdateDomainEvent>,
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IMediator _mediator;

    public MetaMessagesCreator(IMessageRepository messageRepository, IIdGenerator idGenerator, IMediator mediator)
    {
        _messageRepository = messageRepository;
        _idGenerator = idGenerator;
        _mediator = mediator;
    }

    public async Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.MemberInfo.UserId == @event.InitiatorId)
        {
            var metaMessage = new Message(
                type: MessageType.MEMBER_LEAVE,
                id: _idGenerator.CreateId(),
                channelId: @event.Channel.Id,
                author: @event.MemberInfo,
                content: "");
            await _messageRepository.UpsertAsync(metaMessage);
            await _mediator.Publish(new MessageCreateDomainEvent(@event.Channel, metaMessage, @event.MemberInfo));
        }
        else
        {
            var actionInitiatorInfo = @event.Channel.ActiveMembers.First(m => m.UserId == @event.InitiatorId);

            var metaMessage = new Message(
                type: MessageType.MEMBER_REMOVE,
                id: _idGenerator.CreateId(),
                channelId: @event.Channel.Id,
                author: actionInitiatorInfo,
                targetUser: @event.MemberInfo,
                content: "");
            await _messageRepository.UpsertAsync(metaMessage);
            await _mediator.Publish(new MessageCreateDomainEvent(@event.Channel, metaMessage, actionInitiatorInfo));
        }
    }

    public async Task Handle(ChannelTitleUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var actionInitiatorInfo = @event.Channel.ActiveMembers.First(m => m.UserId == @event.InitiatorId);

        var metaMessage = new Message(
            type: MessageType.CHANNEL_TITLE_CHANGE,
            id: _idGenerator.CreateId(),
            channelId: @event.Channel.Id,
            author: actionInitiatorInfo,
            content: "",
            metadata: new ChannelTitleChangeMetadata(@event.Channel.Title));

        await _messageRepository.UpsertAsync(metaMessage);
        await _mediator.Publish(new MessageCreateDomainEvent(@event.Channel, metaMessage, actionInitiatorInfo));
    }

    public async Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var actionInitiatorInfo = @event.Channel.ActiveMembers.First(m => m.UserId == @event.InitiatorId);

        var metaMessage = new Message(
            type: MessageType.MEMBER_ADD,
            id: _idGenerator.CreateId(),
            channelId: @event.Channel.Id,
            author: actionInitiatorInfo,
            targetUser: @event.MemberInfo,
            content: "");

        await _messageRepository.UpsertAsync(metaMessage);
        await _mediator.Publish(new MessageCreateDomainEvent(@event.Channel, metaMessage, actionInitiatorInfo));
    }
}
