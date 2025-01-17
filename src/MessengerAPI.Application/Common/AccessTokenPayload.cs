namespace MessengerAPI.Application.Common;

public record AccessTokenPayload(long UserId, Guid TokenId);
