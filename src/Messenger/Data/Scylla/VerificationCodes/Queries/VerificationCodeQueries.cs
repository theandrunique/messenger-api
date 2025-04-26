using Cassandra;
using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Scylla.VerificationCodes.Queries;

public class VerificationCodeQueries
{
    private readonly PreparedStatement _insert;
    private readonly PreparedStatement _updateAttempts;
    private readonly PreparedStatement _selectByIdentifierAndScenario;
    private readonly PreparedStatement _removeByIdentifierAndScenario;

    public VerificationCodeQueries(ISession session)
    {
        _insert = session.Prepare("""
            INSERT INTO auth.verification_codes (
                identifier,
                scenario,
                codehash,
                timestamp,
                expirestimestamp,
                attempts
            ) VALUES (?, ?, ?, ?, ?, ?) USING TTL ?
            """);

        _updateAttempts = session.Prepare("""
            UPDATE auth.verification_codes
            USING TTL ?
            SET attempts = ?
            WHERE identifier = ? AND scenario = ?
        """);

        _selectByIdentifierAndScenario = session.Prepare("""
            SELECT *
            FROM auth.verification_codes
            WHERE identifier = ? AND scenario = ?
        """);

        _removeByIdentifierAndScenario = session.Prepare("""
            DELETE FROM auth.verification_codes
            WHERE identifier = ? AND scenario = ?
        """);
    }

    public BoundStatement Insert(VerificationCode entity)
    {
        return _insert.Bind(
            entity.Identifier,
            (int)entity.Scenario,
            entity.CodeHash,
            entity.Timestamp,
            entity.ExpiresTimestamp,
            entity.Attempts,
            entity.TTL);
    }

    public BoundStatement UpdateAttempts(VerificationCode entity)
    {
        return _updateAttempts.Bind(
            entity.TTL,
            entity.Attempts,
            entity.Identifier,
            (int)entity.Scenario);
    }

    public BoundStatement SelectByIdentifierAndScenario(string identifier, VerificationCodeScenario scenario)
    {
        return _selectByIdentifierAndScenario.Bind(identifier, (int)scenario);
    }

    public BoundStatement DeleteByIdentifierAndScenario(string identifier, VerificationCodeScenario scenario)
    {
        return _removeByIdentifierAndScenario.Bind(identifier, (int)scenario);
    }
}
