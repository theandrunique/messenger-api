namespace Messenger.Presentation.Schemas.Channels;

public record ForwardMessagesSchema(long targetChannelId, List<long> messages);
