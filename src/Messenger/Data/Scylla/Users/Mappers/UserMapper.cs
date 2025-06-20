using Cassandra;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.Users.Mappers;

public static class UserMapper
{
    public static User Map(Row row)
    {
        return new User(
            id: row.GetValue<long>("id"),
            username: row.GetValue<string>("username"),
            usernameUpdatedTimestamp: row.GetValue<DateTimeOffset>("usernameupdatedtimestamp"),
            passwordHash: row.GetValue<string>("passwordhash"),
            passwordUpdatedTimestamp: row.GetValue<DateTimeOffset>("passwordupdatedtimestamp"),
            terminateSessions: (TimeIntervals)row.GetValue<int>("terminatesessions"),
            bio: row.GetValue<string?>("bio"),
            globalName: row.GetValue<string>("globalname"),
            isActive: row.GetValue<bool>("isactive"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"),
            totpkey: row.GetValue<byte[]>("totpkey"),
            twoFactorAuthentication: row.GetValue<bool>("twofactorauthentication"),
            email: row.GetValue<string>("email"),
            isEmailVerified: row.GetValue<bool>("isemailverified"),
            emailUpdatedTimestamp: row.GetValue<DateTimeOffset>("emailupdatedtimestamp"),
            image: row.GetValue<string?>("image")
        );
    }
}
