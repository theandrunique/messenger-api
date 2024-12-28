using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Common.Interfaces;

public interface IAttachmentService
{
    Task<(string, string)> GenerateUploadUrlAsync(long size, Guid channelId, string filename);
    Task<ErrorOr<Attachment>> ValidateAndCreateAttachmentsAsync(
        string uploadedFilename, string filename,
        Guid channelId,
        CancellationToken cancellationToken);
    Task<ErrorOr<bool>> DeleteAttachmentAsync(string uploadedFilename, CancellationToken cancellationToken);
}

