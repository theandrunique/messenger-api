namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJweHelper
{
    string Encrypt(Dictionary<string, object> payload);
    Dictionary<string, object>? Decrypt(string token);
}
