namespace MessengerAPI.Domain.Channels;

public struct ChannelPermissionSet
{
    private readonly ulong _permissionsValue;

    public ChannelPermissionSet(ulong value)
        => _permissionsValue = value;

    public ChannelPermissionSet(ChannelPermissions permissions)
        => _permissionsValue = (ulong)permissions;

    public bool HasPermission(ChannelPermissions permission)
        => (_permissionsValue & (ulong)permission) != 0;

    public ulong ToValue() => _permissionsValue;
}
