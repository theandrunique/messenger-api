using MediatR;
using Messenger.Core;
using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Data.Messages;
using Messenger.Domain.Events;
using Messenger.Domain.Messages;
using Messenger.Domain.Messages.Metadata;
using Messenger.Domain.Messages.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Messenger.Application.Channels.EventHandlers;

public class MetaMessagesCreator
    : INotificationHandler<ChannelUpdateDomainEvent>,
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>,
      INotificationHandler<ChannelCreateDomainEvent>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IMediator _mediator;
    private readonly ILogger<MetaMessagesCreator> _logger;

    public MetaMessagesCreator(
        IMessageRepository messageRepository,
        IIdGenerator idGenerator,
        IMediator mediator,
        ILogger<MetaMessagesCreator> logger)
    {
        _messageRepository = messageRepository;
        _idGenerator = idGenerator;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.MemberInfo.UserId == @event.InitiatorId)
        {
            await CreateAndPublishMetaMessageAsync(
                type: MessageType.MEMBER_LEAVE,
                channel: @event.Channel,
                author: @event.MemberInfo);
        }
        else
        {
            var actionInitiatorInfo = @event.Channel.ActiveMembers.FirstOrDefault(m => m.UserId == @event.InitiatorId);
            if (actionInitiatorInfo == null)
            {
                LogInitiatorNotFound(@event.Channel, @event.Channel.OwnerId);
                return;
            }

            await CreateAndPublishMetaMessageAsync(
                type: MessageType.MEMBER_REMOVE,
                channel: @event.Channel,
                author: actionInitiatorInfo,
                targetUser: @event.MemberInfo);
        }
    }

    public async Task Handle(ChannelUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var actionInitiatorInfo = @event.Channel.ActiveMembers.FirstOrDefault(m => m.UserId == @event.InitiatorId);
        if (actionInitiatorInfo == null)
        {
            LogInitiatorNotFound(@event.Channel, @event.Channel.OwnerId);
            return;
        }

        if (@event.NewName != null)
        {
            await CreateAndPublishMetaMessageAsync(
                type: MessageType.CHANNEL_NAME_CHANGE,
                channel: @event.Channel,
                author: actionInitiatorInfo,
                metadata: new ChannelNameChangeMessageMetadata(@event.NewName));
        }
        if (@event.NewImage != null)
        {
            await CreateAndPublishMetaMessageAsync(
                type: MessageType.CHANNEL_IMAGE_CHANGE,
                channel: @event.Channel,
                author: actionInitiatorInfo,
                metadata: new ChannelImageChangeMessageMetadata(@event.NewImage));
        }
    }

    public async Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var actionInitiatorInfo = @event.Channel.ActiveMembers.FirstOrDefault(m => m.UserId == @event.InitiatorId);
        if (actionInitiatorInfo == null)
        {
            LogInitiatorNotFound(@event.Channel, @event.Channel.OwnerId);
            return;
        }

        await CreateAndPublishMetaMessageAsync(
            type: MessageType.MEMBER_ADD,
            channel: @event.Channel,
            author: actionInitiatorInfo,
            targetUser: @event.MemberInfo);
    }

    public async Task Handle(ChannelCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Channel.Type != ChannelType.GROUP_DM) return;

        if (@event.Channel.Name == null)
        {
            _logger.LogWarning("Channel name was null for channel {ChannelId}. Skipping creating meta message.", @event.Channel.Id);
            return;
        }

        var actionInitiatorInfo = @event.Channel.ActiveMembers.FirstOrDefault(m => m.UserId == @event.Channel.OwnerId);
        if (actionInitiatorInfo == null)
        {
            LogInitiatorNotFound(@event.Channel, @event.Channel.OwnerId);
            return;
        }

        await CreateAndPublishMetaMessageAsync(
            type: MessageType.CHANNEL_CREATE,
            channel: @event.Channel,
            author: actionInitiatorInfo,
            metadata: new ChannelCreateMessageMetadata(@event.Channel.Name));
    }

    private async Task CreateAndPublishMetaMessageAsync(
        MessageType type,
        Channel channel,
        ChannelMemberInfo author,
        string content = "",
        ChannelMemberInfo? targetUser = null,
        IMessageMetadata? metadata = null)
    {
        var metaMessage = new Message(
            type: type,
            id: _idGenerator.CreateId(),
            channelId: channel.Id,
            author: author,
            content: content,
            targetUser: targetUser,
            metadata: metadata);

        await _messageRepository.UpsertAsync(metaMessage);
        await _mediator.Publish(new MessageCreateDomainEvent(channel, metaMessage, author));
    }

    private void LogInitiatorNotFound(
        Channel channel,
        long? initiatorId)
    {
        _logger.LogError(
            "Initiator {InitiatorId} was not found among active members in channel {ChannelId}. ActiveUsers: {ActiveUserIds}",
            initiatorId,
            channel.Id,
            channel.ActiveMembers.Select(m => m.UserId).ToArray());
    }
}
