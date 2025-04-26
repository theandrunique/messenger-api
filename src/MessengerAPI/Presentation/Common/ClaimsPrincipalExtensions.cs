using System.Security.Claims;
using MessengerAPI.Infrastructure.Auth;

namespace MessengerAPI.Presentation.Common;

public static class ClaimsPrincipalExtensions
{
    public static UserIdentity GetIdentity(this ClaimsPrincipal user)
    {
        var identity = user.Identities.OfType<UserIdentity>().FirstOrDefault();
        if (identity == null)
        {
            throw new Exception("User identity not found.");
        }
        return identity;
    }
}
