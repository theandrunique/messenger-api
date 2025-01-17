namespace MessengerAPI.Application.Common;

public record RefreshTokenPayload(Guid TokenId, long Sub);
