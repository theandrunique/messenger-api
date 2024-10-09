using MessengerAPI.Application.Auth.Common.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace MessengerAPI.Infrastructure.Common;

public class BCryptHelper : IHashHelper
{
    public string Hash(string value)
    {
        return BC.HashPassword(value);
    }

    public bool Verify(string hash, string value)
    {
        return BC.Verify(value, hash);
    }
}
