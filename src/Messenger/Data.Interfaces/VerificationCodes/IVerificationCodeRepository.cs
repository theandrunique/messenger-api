using Messenger.Domain.Entities;
using Messenger.Domain.ValueObjects;

namespace Messenger.Data.Interfaces.VerificationCodes;

public interface IVerificationCodeRepository
{
    Task UpsertAsync(VerificationCode verificationCode);
    Task UpdateAttemptsAsync(VerificationCode verificationCode);
    Task<VerificationCode?> GetByIdentifierOrNullAsync(string identifier, VerificationCodeScenario scenario);
    Task Remove(string identifier, VerificationCodeScenario scenario);
}
