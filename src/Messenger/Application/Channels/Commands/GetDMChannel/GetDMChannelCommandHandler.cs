using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Data.Interfaces.Channels;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Entities;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.GetDMChannel;

public class GetDMChannelCommandHandler : IRequestHandler<GetDMChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetDMChannelCommandHandler(
        IChannelRepository channelRepository,
        IIdGenerator idGenerator,
        IUserRepository userRepository,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _idGenerator = idGenerator;
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(GetDMChannelCommand request, CancellationToken cancellationToken)
    {
        var existedDMChannel = await _channelRepository.GetDMChannelOrNullAsync(request.userId, _clientInfo.UserId);
        if (existedDMChannel is not null)
        {
            return ChannelSchema.From(existedDMChannel);
        }

        var userIdsToFind = request.userId == _clientInfo.UserId
            ? new List<long> { _clientInfo.UserId }
            : new List<long> { request.userId, _clientInfo.UserId };

        var members = (await _userRepository.GetByIdsAsync(userIdsToFind)).ToList();

        if (members.Count != userIdsToFind.Count)
        {
            var membersWasNotFound = userIdsToFind.Except(members.Select(x => x.Id)).ToList();
            return Errors.User.NotFound(membersWasNotFound);
        }

        var newChannel = Channel.CreatePrivate(_idGenerator.CreateId(), members.ToArray());

        await _channelRepository.UpsertAsync(newChannel);

        return ChannelSchema.From(newChannel);
    }
}
