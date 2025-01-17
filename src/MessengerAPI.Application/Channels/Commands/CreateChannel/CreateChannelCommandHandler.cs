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
            return ApiErrors.User.NotFound;
        }

        Channel? channel = null;

        if (request.Type == ChannelType.SavedMessages)
        {
            channel = await CreateSavedMessages(request, members);
        }
        else if (request.Type == ChannelType.Private)
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

    private async Task<Channel> CreateSavedMessages(CreateChannelCommand request, List<User> members)
    {
        if (request.Members.Count != 1)
        {
            throw new ArgumentOutOfRangeException("Members count must be 1 to create SavedMessages channel");
        }

        var savedMessagesChannel = await _channelRepository.GetSavedMessagesChannelOrNullAsync(request.Sub);
        if (savedMessagesChannel is not null)
        {
            return savedMessagesChannel;
        }

        var newSavedMessages = Channel.CreateSavedMessages(_idGenerator.CreateId(), members[0]);

        await _channelRepository.AddAsync(newSavedMessages);

        return newSavedMessages;
    }

    private async Task<Channel> CreatePrivateChannel(CreateChannelCommand request, List<User> members)
    {
        if (request.Members.Count != 2)
        {
            throw new ArgumentOutOfRangeException("Members count must be 2 to create Private channel");
        }

        Channel? existedChannel = await _channelRepository.GetPrivateChannelOrNullByIdsAsync(members[0].Id, members[1].Id);
        if (existedChannel is not null)
        {
            return existedChannel;
        }

        var newChannel = Channel.CreatePrivate(_idGenerator.CreateId(), members[0], members[1]);

        await _channelRepository.AddAsync(newChannel);

        return newChannel;
    }

    private async Task<Channel> CreateGroupChannel(CreateChannelCommand request, List<User> members)
    {
        if (request.Title == null)
        {
            throw new ArgumentException("Title is required for Group channel");
        }

        Channel channel = Channel.CreateGroup(_idGenerator.CreateId(), request.Sub, members, request.Title);

        await _channelRepository.AddAsync(channel);

        return channel;
    }
}
