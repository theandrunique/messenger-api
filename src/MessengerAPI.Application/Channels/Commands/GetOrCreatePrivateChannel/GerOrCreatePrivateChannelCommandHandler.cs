using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;

public class GetOrCreatePrivateChannelCommandHandler : IRequestHandler<GetOrCreatePrivateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetOrCreatePrivateChannelCommandHandler(
        IChannelRepository channelRepository,
        IMapper mapper,
        IIdGenerator idGenerator,
        IUserRepository userRepository,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _idGenerator = idGenerator;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(GetOrCreatePrivateChannelCommand request, CancellationToken cancellationToken)
    {
        var existedDMChannel = await _channelRepository.GetPrivateChannelOrNullAsync(request.userId, _clientInfo.UserId);
        if (existedDMChannel is not null)
        {
            return _mapper.Map<ChannelSchema>(existedDMChannel);
        }

        var userIdsToFind = request.userId == _clientInfo.UserId
            ? new List<long> { _clientInfo.UserId }
            : new List<long> { request.userId, _clientInfo.UserId };

        var members = (await _userRepository.GetByIdsAsync(userIdsToFind)).ToList();

        if (members.Count != userIdsToFind.Count)
        {
            var membersWasNotFound = userIdsToFind.Except(members.Select(x => x.Id)).ToList();
            return ApiErrors.User.NotFoundLotOfUsers(membersWasNotFound);
        }

        var newChannel = Channel.CreatePrivate(_idGenerator.CreateId(), members.ToArray());

        await _channelRepository.UpsertAsync(newChannel);

        return _mapper.Map<ChannelSchema>(newChannel);
    }
}
