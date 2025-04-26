namespace MessengerAPI.Application.Auth.Common;

public record RefreshTokenPayload(Guid TokenId, long Sub);
