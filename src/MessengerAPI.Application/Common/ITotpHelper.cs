namespace MessengerAPI.Application.Common;

public interface ITotpHelper
{
    string GenerateTotp(byte[] secretKey, int step);
    bool Verify(string totp, byte[] secretKey, int step);
}
