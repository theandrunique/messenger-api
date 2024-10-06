namespace MessengerAPI.Application.Common.Interfaces;

public interface IClientInfoProvider
{
    /// <summary>
    /// Get IP address
    /// </summary>
    /// <returns>IP address</returns>
    string GetIpAddress();
    /// <summary>
    /// Get device name
    /// </summary>
    /// <returns>Device name</returns>
    string GetDeviceName();
    /// <summary>
    /// Get client name
    /// </summary>
    /// <returns>Client name</returns>
    string GetClientName();
}
