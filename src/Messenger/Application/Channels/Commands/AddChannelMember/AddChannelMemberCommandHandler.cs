using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Data.Interfaces.Channels;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Events;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AddChannelMember;

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
            return Error.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        if (channel.Type == ChannelType.DM)
        {
            return Error.Channel.InvalidOperationForThisChannelType;
        }

        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_MEMBERS))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_MEMBERS);
        }

        var memberToReturn = channel.FindMember(request.UserId);
        if (memberToReturn != null)
        {
            // Member was in channel before
            if (!memberToReturn.IsLeave)
            {
                return Error.Channel.MemberAlreadyInChannel(memberToReturn.UserId);
            }

            memberToReturn.SetLeaveStatus(false);
            await _channelRepository.UpdateIsLeaveStatus(
                memberToReturn.UserId,
                channel.Id,
                memberToReturn.IsLeave);
            await _publisher.Publish(new ChannelMemberAddDomainEvent(
                channel,
                memberToReturn,
                _clientInfo.UserId));
        }
        else
        {
            // Member added to the channel for the first time
            var newMember = await _userRepository.GetByIdOrNullAsync(request.UserId);
            if (newMember is null)
            {
                return Error.User.NotFound(request.UserId);
            }
            var memberInfo = channel.AddNewMember(newMember);

            await _channelRepository.UpsertChannelMemberAsync(channel.Id, memberInfo);
            await _publisher.Publish(new ChannelMemberAddDomainEvent(
                channel,
                memberInfo,
                _clientInfo.UserId));
        }

        return Unit.Value;
    }
}
