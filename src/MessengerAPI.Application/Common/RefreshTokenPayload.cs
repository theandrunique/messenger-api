namespace MessengerAPI.Application.Common;

/// <summary>
/// Refresh token payload 
/// </summary>
/// <param name="tokenId">Id of refresh token</param>
/// <param name="Sub"><see cref="UserId"/></param>
public record RefreshTokenPayload(Guid tokenId, Guid Sub);
