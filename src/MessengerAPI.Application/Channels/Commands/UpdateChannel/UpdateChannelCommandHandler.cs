using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.Events;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.UpdateChannel;

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
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (channel.Type != ChannelType.GROUP)
        {
            return ApiErrors.Channel.InvalidOperationForThisChannelType;
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermissions.MANAGE_CHANNEL))
        {
            return ApiErrors.Channel.InsufficientPermissions(channel.Id, ChannelPermissions.MANAGE_CHANNEL);
        }

        channel.UpdateChannelTitle(request.Title);

        await _channelRepository.UpdateChannelInfo(channel.Id, request.Title, channel.Image);

        await _mediator.Publish(new ChannelTitleUpdateDomainEvent(channel, request.Title, _clientInfo.UserId));

        return ChannelSchema.From(channel);
    }
}
