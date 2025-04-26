using System.Web;
using Messenger.Application.Auth.Common.Interfaces;
using OtpNet;

namespace Messenger.Infrastructure.Auth;

public class TotpHelper : ITotpHelper
{
    public byte[] GenerateSecretKey(int length)
    {
        return KeyGeneration.GenerateRandomKey(length);
    }

    public bool Verify(string totp, byte[] secretKey, int step, int totpSize)
    {
        var totpL = new Totp(secretKey, step, OtpHashMode.Sha1, totpSize);

        return totpL.VerifyTotp(totp, out _);
    }

    public string CreateOtpAuthUrl(
        byte[] secretKey,
        string issuer,
        string accountName)
    {
        string base32Secret = Base32Encoding.ToString(secretKey);

        var encodedIssuer = HttpUtility.UrlEncode(issuer);
        var encodedAccount = HttpUtility.UrlEncode(accountName);

        return $"otpauth://totp/{encodedIssuer}:{encodedAccount}?secret={base32Secret}&issuer={encodedIssuer}&algorithm=SHA1&digits=6&period=30";
    }
}
