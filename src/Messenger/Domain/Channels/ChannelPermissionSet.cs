namespace Messenger.Domain.Channels;

public struct ChannelPermissionSet
{
    private readonly ulong _permissionsValue;

    public ChannelPermissionSet(ulong value)
        => _permissionsValue = value;

    public ChannelPermissionSet(ChannelPermission permissions)
        => _permissionsValue = (ulong)permissions;

    public bool HasPermission(ChannelPermission permission)
        => (_permissionsValue & (ulong)permission) == (ulong)permission;

    public ulong ToValue() => _permissionsValue;
}
