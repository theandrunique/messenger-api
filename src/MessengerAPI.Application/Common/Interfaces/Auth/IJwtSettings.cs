namespace MessengerAPI.Application.Common.Interfaces.Auth;

public interface IJwtSettings
{
    /// <summary>
    /// Expiration time in minutes
    /// </summary>
    public int ExpiryMinutes { get; }
    /// <summary>
    /// Expiration time in seconds
    /// </summary>
    public int ExpirySeconds => ExpiryMinutes * 60;
}
