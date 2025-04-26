namespace Messenger.Application.Auth.Common.Interfaces;

public interface ITotpHelper
{
    byte[] GenerateSecretKey(int length);
    bool Verify(string totp, byte[] secretKey, int step, int totpSize);
    string CreateOtpAuthUrl(byte[] secretKey, string issuer, string accountName);
}
