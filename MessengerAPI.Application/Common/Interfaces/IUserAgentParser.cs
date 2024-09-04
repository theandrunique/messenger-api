namespace MessengerAPI.Application.Common.Interfaces;

public interface IUserAgentParser
{
    void Parse(string userAgent);
    string GetDeviceName();
    string GetClientName();
}
