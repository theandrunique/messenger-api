using System.Security.Claims;

namespace Messenger.Infrastructure.Auth;

public class UserIdentity : ClaimsIdentity
{
    public UserIdentity(ClaimsPrincipal? claims)
    {
        if (claims == null)
        {
            throw new ArgumentNullException(nameof(claims));
        }

        UserId = claims.GetUserId();
        TokenId = claims.GetTokenId();
    }

    public long UserId { get; private set; }
    public Guid TokenId { get; private set; }
}
