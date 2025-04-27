namespace Messenger.Application.Auth.Common.Interfaces;

public interface IRevokedTokenService
{
    Task<bool> IsTokenRevokedAsync(Guid tokenId);
    Task RevokeTokenAsync(Guid tokenId, TimeSpan expiry);
}
