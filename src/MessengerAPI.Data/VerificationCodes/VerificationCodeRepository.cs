using Cassandra;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.VerificationCodes;

internal class VerificationCodeRepository : IVerificationCodeRepository
{
    private readonly VerificationCodeQueries _queries;
    private readonly ISession _session;

    public VerificationCodeRepository(VerificationCodeQueries queries, ISession session)
    {
        _queries = queries;
        _session = session;
    }

    public Task UpsertAsync(VerificationCode verificationCode)
    {
        return _session.ExecuteAsync(_queries.Insert(verificationCode));
    }

    public Task UpdateAttemptsAsync(VerificationCode verificationCode)
    {
        return _session.ExecuteAsync(_queries.UpdateAttempts(verificationCode));
    }
}
