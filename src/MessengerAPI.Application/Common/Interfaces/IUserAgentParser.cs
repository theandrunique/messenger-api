namespace MessengerAPI.Application.Common.Interfaces;

public interface IUserAgentParser
{
    /// <summary>
    /// Parse user agent
    /// </summary>
    /// <param name="userAgent">User agent</param>
    void Parse(string userAgent);
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
