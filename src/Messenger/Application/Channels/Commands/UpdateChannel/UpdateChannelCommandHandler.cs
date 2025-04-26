using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Channels;
using Messenger.Domain.Events;
using Messenger.Domain.ValueObjects;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.UpdateChannel;

public class UpdateChannelCommandHandler : IRequestHandler<UpdateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMediator _mediator;

    public UpdateChannelCommandHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IMediator mediator)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _mediator = mediator;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel == null)
        {
            return Errors.Channel.NotFound(request.ChannelId);
        }
        if (channel.Type != ChannelType.GROUP_DM)
        {
            return Errors.Channel.InvalidOperationForThisChannelType;
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Errors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_CHANNEL))
        {
            return Errors.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_CHANNEL);
        }

        channel.UpdateChannelName(request.Name);

        await _channelRepository.UpdateChannelInfo(channel.Id, request.Name, channel.Image);

        await _mediator.Publish(new ChannelNameUpdateDomainEvent(channel, request.Name, _clientInfo.UserId));

        return ChannelSchema.From(channel);
    }
}
