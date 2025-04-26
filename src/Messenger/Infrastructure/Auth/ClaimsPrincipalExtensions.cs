using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Messenger.Infrastructure.Auth;

public static class ClaimsPrincipalExtensions
{

    public static bool TryGetUserId(this ClaimsPrincipal user, [NotNullWhen(true)] out long? userId)
    {
        try
        {
            userId = user.GetUserId();
            return true;
        }
        catch (Exception)
        {
            userId = null;
            return false;
        }
    }

    public static bool TryGetTokenId(this ClaimsPrincipal user, [NotNullWhen(true)] out Guid? tokenId)
    {
        try
        {
            tokenId = user.GetTokenId();
            return true;
        }
        catch (Exception)
        {
            tokenId = null;
            return false;
        }
    }

    public static long GetUserId(this ClaimsPrincipal user)
    {
        var subString = user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (long.TryParse(subString, out var sub))
        {
            return sub;
        }

        throw new Exception("User id is not valid or not found.");
    }

    public static Guid GetTokenId(this ClaimsPrincipal user)
    {
        var tokenIdString = user.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (Guid.TryParse(tokenIdString, out var tokenId))
        {
            return tokenId;
        }

        throw new Exception("Token id is not valid or not found.");
    }
}
