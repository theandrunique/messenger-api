using AutoMapper;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;
using Newtonsoft.Json;

namespace MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;

public class GetOrCreatePrivateChannelCommandHandler : IRequestHandler<GetOrCreatePrivateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;
    private readonly IUserRepository _userRepository;

    public GetOrCreatePrivateChannelCommandHandler(
        IChannelRepository channelRepository,
        IMapper mapper,
        IIdGenerator idGenerator,
        IUserRepository userRepository)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _idGenerator = idGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(GetOrCreatePrivateChannelCommand request, CancellationToken cancellationToken)
    {
        var existedDMChannel = await _channelRepository.GetPrivateChannelOrNullAsync(request.userId, request.Sub);
        if (existedDMChannel is not null)
        {
            return _mapper.Map<ChannelSchema>(existedDMChannel);
        }

        var userIdsToFind = request.userId != request.Sub
            ? new List<long> { request.userId, request.Sub }
            : new List<long> { request.Sub };

        var members = (await _userRepository.GetByIdsAsync(userIdsToFind)).ToList();

        if (members.Count != userIdsToFind.Count)
        {
            var membersWasNotFound = userIdsToFind.Except(members.Select(x => x.Id)).ToList();
            return ApiErrors.User.NotFoundLotOfUsers(membersWasNotFound);
        }

        var newChannel = Channel.CreatePrivate(_idGenerator.CreateId(), members.ToArray());

        await _channelRepository.AddAsync(newChannel);

        return _mapper.Map<ChannelSchema>(newChannel);
    }
}
