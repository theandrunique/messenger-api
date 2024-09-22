namespace MessengerAPI.Application.Common.Interfaces;

public interface IJwtSettings
{
    public int ExpiryMinutes { get; }
    public int ExpirySeconds => ExpiryMinutes * 60;
}
