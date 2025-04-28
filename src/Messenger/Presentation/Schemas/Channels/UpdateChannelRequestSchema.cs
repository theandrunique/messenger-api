namespace Messenger.Presentation.Schemas.Channels;

public record UpdateChannelRequestSchema(
    string? name,
    IFormFile? image);
