using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddChannelMember;

public class AddChannelMemberCommandHandler : IRequestHandler<AddChannelMemberCommand, ErrorOr<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;

    public AddChannelMemberCommandHandler(
        IUserRepository userRepository,
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo)
    {
        _userRepository = userRepository;
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<Unit>> Handle(AddChannelMemberCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.IsUserInTheChannel(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var newMember = await _userRepository.GetByIdOrNullAsync(request.UserId);
        if (newMember is null)
        {
            return ApiErrors.User.NotFound(request.UserId);
        }

        if (channel.Type == ChannelType.Private)
        {
            return ApiErrors.Channel.InvalidOperationForChannelType(channel.Id);
        }

        if (channel.OwnerId != _clientInfo.UserId)
        {
            return ApiErrors.Channel.NotOwner(channel.Id);
        }

        if (channel.IsUserInTheChannel(newMember.Id))
        {
            return ApiErrors.Channel.MemberAlreadyInChannel(newMember.Id);
        }

        var memberInfo = channel.AddNewMember(newMember);

        await _channelRepository.AddMemberToChannel(request.ChannelId, memberInfo);

        return await Unit.Task;
    }
}
