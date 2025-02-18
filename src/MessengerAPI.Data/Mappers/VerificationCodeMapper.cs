using Cassandra;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.Mappers;

internal static class VerificationCodeMapper
{
    public static VerificationCode Map(Row row)
    {
        return new VerificationCode(
            identifier: row.GetValue<string>("identifier"),
            scenario: (VerificationCodeScenario)row.GetValue<int>("scenario"),
            codeHash: row.GetValue<string>("codehash"),
            timestamp: row.GetValue<DateTimeOffset>("timestamp"),
            expiresTimestamp: row.GetValue<DateTimeOffset>("expirestimestamp"),
            attempts: row.GetValue<int>("attempts")
        );
    }
}