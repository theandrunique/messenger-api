using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetAttachments;

public record GetAttachmentsQuery(
    long ChannelId,
    long Before,
    int Limit) : IRequest<ErrorOr<List<AttachmentSchema>>>;
