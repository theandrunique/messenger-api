using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Data.VerificationCodes;

public interface IVerificationCodeRepository
{
    Task UpsertAsync(VerificationCode verificationCode);
    Task UpdateAttemptsAsync(VerificationCode verificationCode);
    Task<VerificationCode?> GetByIdentifierOrNullAsync(string identifier, VerificationCodeScenario scenario);
    Task Remove(string identifier, VerificationCodeScenario scenario);
}
