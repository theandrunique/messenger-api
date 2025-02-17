using MessengerAPI.Domain.Entities;

namespace MessengerAPI.Data.VerificationCodes;

public interface IVerificationCodeRepository
{
    Task UpsertAsync(VerificationCode verificationCode);
}
