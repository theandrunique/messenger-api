using Cassandra;
using Messenger.Domain.Auth;
using Messenger.Domain.Auth.ValueObjects;

namespace Messenger.Data.Scylla.Users.Mappers;

public static class UserMapper
{
    public static User Map(Row row)
    {
        return new User(
            id: row.GetValue<long>("user_id"),
            username: row.GetValue<string>("username"),
            usernameUpdatedTimestamp: row.GetValue<DateTimeOffset>("username_updated_timestamp"),
            passwordHash: row.GetValue<string>("password_hash"),
            passwordUpdatedTimestamp: row.GetValue<DateTimeOffset>("password_updated_timestamp"),
            sessionsLifetime: (SessionLifetime)row.GetValue<int>("sessions_lifetime"),
            bio: row.GetValue<string?>("bio"),
            globalName: row.GetValue<string>("global_name"),
            isActive: row.GetValue<bool>("is_active"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"),
            totpkey: row.GetValue<byte[]>("totp_key"),
            twoFactorAuthentication: row.GetValue<bool>("two_factor_authentication"),
            email: row.GetValue<string>("email"),
            isEmailVerified: row.GetValue<bool>("is_email_verified"),
            emailUpdatedTimestamp: row.GetValue<DateTimeOffset>("email_updated_timestamp"),
            image: row.GetValue<string?>("image")
        );
    }
}
