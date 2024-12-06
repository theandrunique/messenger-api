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
        var totp = new Totp(secretKey, step, OtpHashMode.Sha512, totpSize);

        return totp.ComputeTotp();
    }

    public bool Verify(string totp, byte[] secretKey, int step, int totpSize)
    {
        var totpL = new Totp(secretKey, step, OtpHashMode.Sha512, totpSize);

        return totpL.VerifyTotp(totp, out _);
    }
}
