using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.Events;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.RemoveChannelMember;

public class RemoveChannelMemberCommandHandler : IRequestHandler<RemoveChannelMemberCommand, ErrorOr<Unit>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;

    public RemoveChannelMemberCommandHandler(IChannelRepository channelRepository, IClientInfoProvider clientInfo, IPublisher publisher)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _publisher = publisher;
    }

    public async Task<ErrorOr<Unit>> Handle(RemoveChannelMemberCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        if (channel.Type == ChannelType.PRIVATE)
        {
            return ApiErrors.Channel.InvalidOperationForThisChannelType;
        }

        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermissions.MANAGE_MEMBERS))
        {
            return ApiErrors.Channel.InsufficientPermissions(channel.Id, ChannelPermissions.MANAGE_MEMBERS);
        }

        if (!channel.HasMember(request.UserId))
        {
            return ApiErrors.Channel.UserNotMember(request.UserId, channel.Id);
        }

        var memberInfo = channel.RemoveMember(request.UserId);

        await _channelRepository.RemoveMemberFromChannel(request.ChannelId, request.UserId);

        await _publisher.Publish(new ChannelMemberRemoveDomainEvent(channel, memberInfo, _clientInfo.UserId));

        return await Unit.Task;
    }
}
