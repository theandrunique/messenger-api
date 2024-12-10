using System.Web;
using MessengerAPI.Application.Common;
using OtpNet;

namespace MessengerAPI.Infrastructure.Common;

public class TotpHelper : ITotpHelper
{
    public byte[] GenerateSecretKey(int length)
    {
        return KeyGeneration.GenerateRandomKey(length);
    }

    public string GenerateTotp(byte[] secretKey, int step, int totpSize)
    {
        var totp = new Totp(secretKey, step, OtpHashMode.Sha1, totpSize);

        return totp.ComputeTotp(DateTime.UtcNow);
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
