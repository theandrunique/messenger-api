using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.Common.Errors;

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

    public async Task<ErrorOr<ChannelSchema>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (request.Members.Count == 1 && request.Members[0] == request.Sub)
        {
            var member = await _userRepository.GetByIdAsync(request.Sub);
            if (member == null)
            {
                return UserErrors.NotFound;
            }
            var savedMessages = await _channelRepository.GetSavedMessagesAsync(request.Sub);
            if (savedMessages is not null)
            {
                return _mapper.Map<ChannelSchema>(savedMessages);
            }
            var newSavedMessages = Channel.CreateSavedMessages(member);
            await _channelRepository.AddAsync(newSavedMessages);
            await _channelRepository.Commit();

            return _mapper.Map<ChannelSchema>(newSavedMessages);
        }

        request.Members.Add(request.Sub);
        var members = await _userRepository.GetByIdsAsync(request.Members);
        if (members.Count != request.Members.Count)
        {
            return UserErrors.NotFound;
        }

        if (request.Members.Count == 2)
        {
            Channel? existedChannel = await _channelRepository.GetPrivateChannelAsync(request.Members[0], request.Sub);
            if (existedChannel is not null)
            {
                return _mapper.Map<ChannelSchema>(existedChannel);
            }

            var newChannel = Channel.CreatePrivate(members[0], members[1]);

            await _channelRepository.AddAsync(newChannel);
            await _channelRepository.Commit();

            return _mapper.Map<ChannelSchema>(newChannel);
        }

        var newGroupChannel = Channel.CreateGroup(request.Sub, members, request.Title);

        await _channelRepository.AddAsync(newGroupChannel);
        await _channelRepository.Commit();

        return _mapper.Map<ChannelSchema>(newGroupChannel);
    }
}
