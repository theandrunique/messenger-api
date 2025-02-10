using DeviceDetectorNET;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Infrastructure.Auth;
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
            throw new Exception("ClientInfoProvider was expected to be used in the context of an http request");
        }

        _deviceDetector = new DeviceDetector();
        _httpContext = httpContextAccessor.HttpContext;

        var userAgent = _httpContext.Request.Headers.UserAgent.ToString();
        _deviceDetector.SetUserAgent(userAgent);
        _deviceDetector.Parse();
    }
    public string IpAddress
    {
        get
        {
            string? ipAddress = _httpContext.Connection.RemoteIpAddress?.ToString();
            return ipAddress ?? throw new Exception("IP address expected to be not null");
        }
    }

    public string DeviceName => _deviceDetector.GetDeviceName();

    public string ClientName
    {
        get
        {
            var client = _deviceDetector.GetClient();
            return $"{client.Match.Name} {client.Match.Version}";
        }
    }

    public long UserId => _httpContext.User.GetUserId();
    public Guid TokenId => _httpContext.User.GetTokenId();
}
