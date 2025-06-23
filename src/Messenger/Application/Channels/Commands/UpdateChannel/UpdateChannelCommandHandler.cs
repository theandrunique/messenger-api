using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Domain.Events;
using Messenger.Errors;
using Messenger.Application.Channels.Common;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Data.Channels;

namespace Messenger.Application.Channels.Commands.UpdateChannel;

public class UpdateChannelCommandHandler : IRequestHandler<UpdateChannelCommand, ErrorOr<ChannelSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMediator _mediator;
    private readonly ChannelImageService _channelImageService;
    private readonly IChannelLoaderFactory _channelLoaderFactory;

    public UpdateChannelCommandHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IMediator mediator,
        ChannelImageService channelImageService,
        IChannelLoaderFactory channelLoaderFactory)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _mediator = mediator;
        _channelImageService = channelImageService;
        _channelLoaderFactory = channelLoaderFactory;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(UpdateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelLoaderFactory
            .CreateLoader()
            .WithId(request.ChannelId)
            .WithMember(_clientInfo.UserId)
            .LoadAsync();

        if (channel == null)
        {
            return Error.Channel.NotFound(request.ChannelId);
        }
        if (channel.Type != ChannelType.GROUP_DM)
        {
            return Error.Channel.InvalidOperationForThisChannelType;
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.MemberHasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_CHANNEL))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_CHANNEL);
        }

        var domainEvent = new ChannelUpdateDomainEvent(channel, _clientInfo.UserId);

        if (request.Name != null)
        {
            channel.UpdateName(request.Name);
            domainEvent.NewName = channel.Name;
        }
        if (request.Image != null)
        {
            var imageHash = await _channelImageService.UploadImage(request.Image, channel.Id, cancellationToken);
            channel.UpdateImage(imageHash);
            domainEvent.NewImage = imageHash;
        }

        await _channelRepository.UpdateChannelInfo(channel.Id, domainEvent);

        await _mediator.Publish(domainEvent);

        return ChannelSchema.From(channel);
    }
}
