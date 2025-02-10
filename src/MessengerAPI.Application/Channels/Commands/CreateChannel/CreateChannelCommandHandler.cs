using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
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
    private readonly IClientInfoProvider _clientInfo;

    public CreateChannelCommandHandler(
        IChannelRepository channelRepository,
        IUserRepository userRepository,
        IMapper mapper,
        IIdGenerator idGenerator,
        IGatewayService gateway,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _idGenerator = idGenerator;
        _gateway = gateway;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        if (!request.Members.Contains(_clientInfo.UserId))
        {
            request.Members.Add(_clientInfo.UserId);
        }

        var members = (await _userRepository.GetByIdsAsync(request.Members)).ToList();

        if (members.Count != request.Members.Count)
        {
            var membersWasNotFound = request.Members.Except(members.Select(x => x.Id)).ToList();
            return ApiErrors.User.NotFoundLotOfUsers(membersWasNotFound);
        }

        Channel channel = Channel.CreateGroup(_idGenerator.CreateId(), _clientInfo.UserId, request.Title, members.ToArray());

        await _channelRepository.UpsertAsync(channel);

        var channelSchema = _mapper.Map<ChannelSchema>(channel);

        await _gateway.PublishAsync(new ChannelCreateGatewayEvent(channelSchema));

        return channelSchema;
    }
}
