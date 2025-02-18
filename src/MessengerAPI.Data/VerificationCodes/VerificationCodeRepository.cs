using Cassandra;
using MessengerAPI.Data.Mappers;
using MessengerAPI.Data.Queries;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

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
    public async Task<VerificationCode?> GetByIdentifierOrNullAsync(
        string identifier,
        VerificationCodeScenario scenario)
    {
        var result = (await _session
            .ExecuteAsync(_queries.SelectByIdentifierAndScenario(identifier, scenario)))
            .FirstOrDefault();

        if (result == null)
        {
            return null;
        }

        return VerificationCodeMapper.Map(result);
    }

    public Task Remove(string identifier, VerificationCodeScenario scenario)
    {
        return _session.ExecuteAsync(_queries.DeleteByIdentifierAndScenario(identifier, scenario));
    }
}
