using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
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

        var newMember = await _userRepository.GetByIdOrNullAsync(request.UserId);
        if (newMember is null)
        {
            return ApiErrors.User.NotFound(request.UserId);
        }

        if (channel.Type == ChannelType.PRIVATE)
        {
            return ApiErrors.Channel.InvalidOperationForChannelType;
        }

        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermissions.MANAGE_MEMBERS))
        {
            return ApiErrors.Channel.InsufficientPermissions(channel.Id, ChannelPermissions.MANAGE_MEMBERS.ToString());
        }

        if (channel.HasMember(newMember.Id))
        {
            return ApiErrors.Channel.MemberAlreadyInChannel(newMember.Id);
        }

        var memberInfo = channel.AddNewMember(newMember, ChannelPermissions.DEFAULT);

        await _channelRepository.AddMemberToChannel(request.ChannelId, memberInfo);

        await _publisher.Publish(new ChannelMemberAddDomainEvent(channel, memberInfo, _clientInfo.UserId));

        return await Unit.Task;
    }
}
