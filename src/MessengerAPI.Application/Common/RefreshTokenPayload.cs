namespace MessengerAPI.Application.Common;

public record RefreshTokenPayload(Guid tokenId, Guid Sub);
