using Messenger.Domain.ValueObjects;

namespace Messenger.Domain.Auth;

public class VerificationCode
{
    public string Identifier { get; private set; }
    public VerificationCodeScenario Scenario { get; private set; }
    public string CodeHash { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }
    public DateTimeOffset ExpiresTimestamp { get; private set; }
    public int Attempts { get; private set; }
    public int TTL => (int)(ExpiresTimestamp - Timestamp).TotalSeconds;

    public VerificationCode(
        string identifier,
        VerificationCodeScenario scenario,
        string codeHash,
        DateTimeOffset expiresTimestamp)
    {
        Identifier = identifier;
        Scenario = scenario;
        CodeHash = codeHash;
        Timestamp = DateTimeOffset.UtcNow;
        ExpiresTimestamp = expiresTimestamp;
        Attempts = 0;
    }

    public VerificationCode(
        string identifier,
        VerificationCodeScenario scenario,
        string codeHash,
        DateTimeOffset timestamp,
        DateTimeOffset expiresTimestamp,
        int attempts)
    {
        Identifier = identifier;
        Scenario = scenario;
        CodeHash = codeHash;
        Timestamp = timestamp;
        ExpiresTimestamp = expiresTimestamp;
        Attempts = attempts;
    }

    public void IncrementAttemptsCount() => Attempts++;
}
