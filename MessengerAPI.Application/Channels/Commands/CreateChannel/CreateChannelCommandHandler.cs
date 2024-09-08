using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<Channel>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;

    public CreateChannelCommandHandler(IChannelRepository channelRepository, IUserRepository userRepository)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Channel>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
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
                return savedMessages;
            }
            var newSavedMessages = Channel.CreateSavedMessages(member);
            await _channelRepository.AddAsync(newSavedMessages);
            await _channelRepository.Commit();
            return newSavedMessages;
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
                return existedChannel;
            }

            var newChannel = Channel.CreatePrivate(members[0], members[1]);

            await _channelRepository.AddAsync(newChannel);
            await _channelRepository.Commit();

            return newChannel;
        }

        var newGroupChannel = Channel.CreateGroup(request.Sub, members, request.Title);

        await _channelRepository.AddAsync(newGroupChannel);
        await _channelRepository.Commit();

        return newGroupChannel;
    }
}
