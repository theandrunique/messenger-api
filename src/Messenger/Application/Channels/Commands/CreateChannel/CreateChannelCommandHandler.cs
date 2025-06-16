using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Data.Interfaces.Channels;
using Messenger.Data.Interfaces.Users;
using Messenger.Domain.Channels;
using Messenger.Domain.Events;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands;

public class CreateChannelCommandHandler : IRequestHandler<CreateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;

    public CreateChannelCommandHandler(
        IChannelRepository channelRepository,
        IUserRepository userRepository,
        IIdGenerator idGenerator,
        IClientInfoProvider clientInfo,
        IPublisher publisher)
    {
        _channelRepository = channelRepository;
        _userRepository = userRepository;
        _idGenerator = idGenerator;
        _clientInfo = clientInfo;
        _publisher = publisher;
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
            return Error.User.NotFound(membersWasNotFound);
        }

        Channel channel = Channel.CreateGroup(
            id: _idGenerator.CreateId(),
            ownerId: _clientInfo.UserId,
            name: request.Name,
            members: members.ToArray());

        await _channelRepository.UpsertAsync(channel);

        await _publisher.Publish(new ChannelCreateDomainEvent(channel));

        return ChannelSchema.From(channel);
    }
}
