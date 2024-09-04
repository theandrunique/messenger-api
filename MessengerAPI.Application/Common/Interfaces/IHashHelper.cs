namespace MessengerAPI.Application.Common.Interfaces;

public interface IHashHelper
{
    string Hash(string value);

    bool Verify(string hash, string value);
}
