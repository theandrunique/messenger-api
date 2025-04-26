namespace Messenger.Application.Auth.Common;

public record AccessTokenPayload(long UserId, Guid TokenId);
