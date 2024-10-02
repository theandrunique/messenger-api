using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Domain.UserAggregate;

namespace MessengerAPI.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateChannelCommandHandler(IChannelRepository channelRepository, IUserRepository userRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Create new channel, if private checks for existing channels and returns it, otherwise creates new one
    /// </summary>
    /// <param name="request"><see cref="CreateChannelCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="ChannelSchema"/></returns>
    /// <exception cref="NotImplementedException">If channel type is not implemented</exception>
    public async Task<ErrorOr<ChannelSchema>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (!request.Members.Contains(request.Sub))
        {
            request.Members.Add(request.Sub);
        }

        var members = await _userRepository.GetByIdsAsync(request.Members, cancellationToken);
        if (members.Count != request.Members.Count)
        {
            return Errors.User.NotFound;
        }

        Channel? channel = null;

        if (request.Type == ChannelType.Private)
        {
            channel = await CreatePrivateChannel(request, members, cancellationToken);
        }
        else if (request.Type == ChannelType.Group)
        {
            channel = await CreateGroupChannel(request, members, cancellationToken);
        }


        if (channel == null)
            throw new NotImplementedException($"Channel type {request.Type} is not implemented");

        return _mapper.Map<ChannelSchema>(channel);
    }

    private async Task<Channel> CreatePrivateChannel(CreateChannelCommand request, List<User> members, CancellationToken token)
    {
        if (request.Members.Count == 1)
        {
            var savedMessagesChannel = await _channelRepository.GetSavedMessagesChannelOrNullAsync(request.Sub, token);
            if (savedMessagesChannel is not null)
            {
                return savedMessagesChannel;
            }

            var newSavedMessages = Channel.CreateSavedMessages(members[0]);

            await _channelRepository.AddAsync(newSavedMessages, token);
            await _channelRepository.CommitAsync(token);

            return newSavedMessages;
        }
        if (request.Members.Count == 2)
        {
            Channel? existedChannel = await _channelRepository.GetPrivateChannelOrNullAsync(members[0].Id, request.Sub, token);
            if (existedChannel is not null)
            {
                return existedChannel;
            }

            var newChannel = Channel.CreatePrivate(members[0], members[1]);

            await _channelRepository.AddAsync(newChannel, token);
            await _channelRepository.CommitAsync(token);

            return newChannel;
        }
        throw new ArgumentOutOfRangeException("Members count must be 1 or 2 to create Private channel");
    }

    private async Task<Channel> CreateGroupChannel(CreateChannelCommand request, List<User> members, CancellationToken token)
    {
        Channel channel = Channel.CreateGroup(request.Sub, members, request.Title);

        await _channelRepository.AddAsync(channel, token);
        await _channelRepository.CommitAsync(token);

        return channel;
    }
}
