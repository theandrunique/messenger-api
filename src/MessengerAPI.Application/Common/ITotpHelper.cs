namespace MessengerAPI.Application.Common;

public interface ITotpHelper
{
    byte[] GenerateSecretKey(int length);
    string GenerateTotp(byte[] secretKey, int step, int totpSize);
    bool Verify(string totp, byte[] secretKey, int step, int totpSize);
    string CreateOtpAuthUrl(byte[] secretKey, string issuer, string accountName);
}
