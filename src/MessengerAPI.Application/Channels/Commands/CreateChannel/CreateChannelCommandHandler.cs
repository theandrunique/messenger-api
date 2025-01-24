using AutoMapper;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;

    public CreateChannelCommandHandler(
        IChannelRepository channelRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IIdGenerator idGenerator)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (!request.Members.Contains(request.Sub))
        {
            request.Members.Add(request.Sub);
        }

        var members = (await _userRepository.GetByIdsAsync(request.Members)).ToList();

        if (members.Count() != request.Members.Count)
        {
            var membersWasNotFound = request.Members.Except(members.Select(x => x.Id)).ToList();
            return ApiErrors.User.NotFoundLotOfUsers(membersWasNotFound);
        }

        Channel? channel = null;

        if (request.Type == ChannelType.Private)
        {
            channel = await CreatePrivateChannel(request, members);
        }
        else if (request.Type == ChannelType.Group)
        {
            channel = await CreateGroupChannel(request, members);
        }

        if (channel == null)
        {
            throw new NotImplementedException($"Channel type {request.Type} is not implemented");
        }

        return _mapper.Map<ChannelSchema>(channel);
    }

    private async Task<Channel> CreatePrivateChannel(CreateChannelCommand request, List<User> members)
    {
        if (request.Members.Count != 2 || request.Members.Count != 1)
        {
            throw new ArgumentOutOfRangeException("Members count must be 2 or 1 to create Private channel");
        }

        Channel? existedChannel;
        if (request.Members.Count == 1)
        {
            existedChannel = await _channelRepository.GetPrivateChannelOrNullByIdsAsync(members[0].Id, members[1].Id);
        }
        else
        {
            existedChannel = await _channelRepository.GetPrivateChannelOrNullByIdsAsync(members[0].Id, members[0].Id);
        }

        if (existedChannel is not null)
        {
            return existedChannel;
        }

        var newChannel = Channel.CreatePrivate(_idGenerator.CreateId(), members.ToArray());

        await _channelRepository.AddAsync(newChannel);

        return newChannel;
    }

    private async Task<Channel> CreateGroupChannel(CreateChannelCommand request, List<User> members)
    {
        if (request.Title == null)
        {
            throw new ArgumentException("Title is required for Group channel");
        }

        Channel channel = Channel.CreateGroup(_idGenerator.CreateId(), request.Sub, request.Title, members.ToArray());

        await _channelRepository.AddAsync(channel);

        return channel;
    }
}
