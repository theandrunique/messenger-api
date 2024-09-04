namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJweHelper
{
    string Encrypt(RefreshTokenPayload payload);
    RefreshTokenPayload? Decrypt(string token);
}
