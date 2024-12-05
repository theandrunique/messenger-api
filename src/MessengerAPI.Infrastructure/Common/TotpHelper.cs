using MessengerAPI.Application.Common;
using OtpNet;

namespace MessengerAPI.Infrastructure.Common;

public class TotpHelper : ITotpHelper
{
    public string GenerateTotp(byte[] secretKey, int step)
    {
        var totp = new Totp(secretKey, step, OtpHashMode.Sha512);

        return totp.ComputeTotp();
    }

    public bool Verify(string totp, byte[] secretKey, int step)
    {
        var totpL = new Totp(secretKey, step, OtpHashMode.Sha512);

        return totpL.VerifyTotp(totp, out _);
    }
}
