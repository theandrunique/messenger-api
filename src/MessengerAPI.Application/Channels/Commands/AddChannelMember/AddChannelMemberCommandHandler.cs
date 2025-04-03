using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Interfaces.Channels;
using MessengerAPI.Data.Interfaces.Users;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.Events;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddChannelMember;

public class AddChannelMemberCommandHandler : IRequestHandler<AddChannelMemberCommand, ErrorOr<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;

    public AddChannelMemberCommandHandler(
        IUserRepository userRepository,
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IPublisher publisher)
    {
        _userRepository = userRepository;
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _publisher = publisher;
    }

    public async Task<ErrorOr<Unit>> Handle(AddChannelMemberCommand request, CancellationToken cancellationToken)
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

        var memberToReturn = channel.FindMember(request.UserId);
        if (memberToReturn != null)
        {
            // Member was in channel before
            if (!memberToReturn.IsLeave)
            {
                return ApiErrors.Channel.MemberAlreadyInChannel(memberToReturn.UserId);
            }

            memberToReturn.SetLeaveStatus(false);
            await _channelRepository.UpdateIsLeaveStatus(memberToReturn.UserId, channel.Id, memberToReturn.IsLeave);
            await _publisher.Publish(new ChannelMemberAddDomainEvent(channel, memberToReturn, _clientInfo.UserId));
        }
        else
        {
            // Member added to the channel for the first time
            var newMember = await _userRepository.GetByIdOrNullAsync(request.UserId);
            if (newMember is null)
            {
                return ApiErrors.User.NotFound(request.UserId);
            }
            var memberInfo = channel.AddNewMember(newMember, ChannelPermissions.DEFAULT_MEMBER);

            await _channelRepository.UpsertChannelMemberAsync(channel.Id, memberInfo);
            await _publisher.Publish(new ChannelMemberAddDomainEvent(channel, memberInfo, _clientInfo.UserId));
        }

        return await Unit.Task;
    }
}
