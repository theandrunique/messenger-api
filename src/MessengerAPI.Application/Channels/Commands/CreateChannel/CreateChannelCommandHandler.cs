using AutoMapper;
using MassTransit.Internals;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Events;
using MessengerAPI.Errors;
using MessengerAPI.Gateway;

namespace MessengerAPI.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IIdGenerator _idGenerator;
    private readonly IGatewayService _gateway;

    public CreateChannelCommandHandler(
        IChannelRepository channelRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IIdGenerator idGenerator,
        IGatewayService gateway)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _idGenerator = idGenerator;
        _gateway = gateway;
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

        await _channelRepository.UpsertAsync(channel);

        await _gateway.PublishAsync(new ChannelCreated
        {
            Channel = channel
        });

        return _mapper.Map<ChannelSchema>(channel);
    }
}
