namespace MessengerAPI.Application.Common;

public interface IHashHelper
{
    string Hash(string value);

    bool Verify(string hash, string value);
}
