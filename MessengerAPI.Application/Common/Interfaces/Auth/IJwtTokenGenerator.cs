namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtTokenGenerator
{
    string Generate(string kek);
}
