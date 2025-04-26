using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetAttachments;

public record GetAttachmentsQuery(
    long ChannelId,
    long Before,
    int Limit) : IRequest<ErrorOr<List<AttachmentSchema>>>;
