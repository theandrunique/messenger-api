using DeviceDetectorNET;
using MessengerAPI.Application.Common.Interfaces;

namespace MessengerAPI.Infrastructure.Common;

public class UserAgentParser : IUserAgentParser
{
    private DeviceDetector _deviceDetector;
    public UserAgentParser()
    {
        _deviceDetector = new DeviceDetector();
    }
    public void Parse(string userAgent)
    {
        _deviceDetector.SetUserAgent(userAgent);
        _deviceDetector.Parse();
    }
    public string GetDeviceName()
    {
        return _deviceDetector.GetDeviceName();
    }
    public string GetClientName()
    {
        var client = _deviceDetector.GetClient();
        return $"{client.Match.Name} {client.Match.Version}";
    }
}