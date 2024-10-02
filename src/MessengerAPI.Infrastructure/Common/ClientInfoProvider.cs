using DeviceDetectorNET;
using MessengerAPI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MessengerAPI.Infrastructure.Common;

public class ClientInfoProvider : IClientInfoProvider
{
    private readonly HttpContext _httpContext;
    private readonly DeviceDetector _deviceDetector;

    public ClientInfoProvider(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new Exception("UserAgentParser was expected to be used in the context of an http request");
        }

        _deviceDetector = new DeviceDetector();
        _httpContext = httpContextAccessor.HttpContext;

        var userAgent = _httpContext.Request.Headers.UserAgent.ToString();
        _deviceDetector.SetUserAgent(userAgent);
        _deviceDetector.Parse();
    }
    public string GetIpAddress()
    {
        string? ipAddress = _httpContext.Connection.RemoteIpAddress?.ToString();

        if (ipAddress == null)
        {
            throw new Exception("IP address expected to be not null");
        }
        return ipAddress;
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