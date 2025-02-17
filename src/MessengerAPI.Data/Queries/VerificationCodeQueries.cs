using Cassandra;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.Queries;

internal class VerificationCodeQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _updateAttempts;

    public VerificationCodeQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO verification_codes (
                identifier,
                scenario,
                codehash,
                timestamp,
                expirestimestamp,
                attempts
            ) VALUES (?, ?, ?, ?, ?, ?) USING TTL ?
            """);
        
        _updateAttempts = session.Prepare("""
            UPDATE verification_codes SET attempts = ? WHERE identifier = ? AND scenario = ? USING TTL ?
        """);
    }

    public BoundStatement Insert(VerificationCode entity)
    {
        return _insert.Bind(
            entity.Identifier,
            entity.Scenario.ToString(),
            entity.CodeHash,
            entity.Timestamp,
            entity.ExpiresTimestamp,
            entity.TTL);
    }

    public BoundStatement UpdateAttempts(VerificationCode entity)
    {
        return _updateAttempts.Bind(
            entity.Attempts,
            entity.Identifier,
            entity.Scenario.ToString(),
            entity.TTL);
    }
}
