namespace MessengerAPI.Application.Common.Interfaces;

public interface IClientInfoProvider
{
    string IpAddress { get; }
    string DeviceName { get; }
    string ClientName { get; }
    long UserId { get; }
    Guid TokenId { get; }
}
