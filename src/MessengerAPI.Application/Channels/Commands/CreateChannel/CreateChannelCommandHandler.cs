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

        if (members.Count != request.Members.Count)
        {
            var membersWasNotFound = request.Members.Except(members.Select(x => x.Id)).ToList();
            return ApiErrors.User.NotFoundLotOfUsers(membersWasNotFound);
        }

        Channel channel = Channel.CreateGroup(_idGenerator.CreateId(), request.Sub, request.Title, members.ToArray());

        await _channelRepository.AddAsync(channel);

        return _mapper.Map<ChannelSchema>(channel);
    }
}
