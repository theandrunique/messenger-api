using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Data.Channels;
using Messenger.Domain.Events;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.RemoveChannelMember;

public class RemoveChannelMemberCommandHandler : IRequestHandler<RemoveChannelMemberCommand, ErrorOr<Unit>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IChannelLoaderFactory _channelLoaderFactory;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;

    public RemoveChannelMemberCommandHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IPublisher publisher,
        IChannelLoaderFactory channelLoaderFactory)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _publisher = publisher;
        _channelLoaderFactory = channelLoaderFactory;
    }

    public async Task<ErrorOr<Unit>> Handle(RemoveChannelMemberCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelLoaderFactory
            .CreateLoader()
            .WithId(request.ChannelId)
            .WithMember(_clientInfo.UserId)
            .WithMember(request.UserId)
            .LoadAsync();

        if (channel is null)
        {
            return Error.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        if (channel.Type != ChannelType.GROUP_DM)
        {
            return Error.Channel.InvalidOperationForThisChannelType;
        }

        if (!channel.MemberHasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_MEMBERS))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_MEMBERS);
        }

        var memberToRemove = channel.ActiveMembers.FirstOrDefault(m => m.UserId == request.UserId);
        if (memberToRemove is null)
        {
            // user has never been in this channel
            return Error.Channel.UserNotMember(request.UserId, channel.Id);
        }
        if (memberToRemove.IsLeave)
        {
            // user has been in this channel but left
            return Error.Channel.UserNotMember(request.UserId, channel.Id);
        }

        // We are not setting this, because we need to send a gateway event to user that was removed
        // memberToRemove.SetLeaveStatus(true);

        await _channelRepository.UpdateIsLeaveStatus(memberToRemove.UserId, request.ChannelId, isLeave: true);

        await _publisher.Publish(new ChannelMemberRemoveDomainEvent(channel, memberToRemove, _clientInfo.UserId));

        return await Unit.Task;
    }
}
