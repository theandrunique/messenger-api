using System.IO.Pipelines;
using System.Security.Cryptography;
using MessengerAPI.Application.Auth.Common.Interfaces;
using MessengerAPI.Data.VerificationCodes;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.ValueObjects;

namespace MessengerAPI.Application.Users.Common;

public class VerificationCodeService
{
    private readonly IHashHelper _hashHelper;
    private readonly IVerificationCodeRepository _verificationCodeRepository;

    public VerificationCodeService(
        IHashHelper hashHelper,
        IVerificationCodeRepository verificationCodeRepository)
    {
        _hashHelper = hashHelper;
        _verificationCodeRepository = verificationCodeRepository;
    }

    public async Task<(string otp, VerificationCode code)> CreateAsync(
        long userId,
        VerificationCodeScenario scenario,
        TimeSpan timeToExpire)
    {
        var otp = GenerateOtp();

        var code = new VerificationCode(
            identifier: GetCodeIdentifier(userId, scenario),
            scenario: scenario,
            codeHash: _hashHelper.Hash(otp),
            expiresTimestamp: DateTimeOffset.UtcNow.Add(timeToExpire));

        await _verificationCodeRepository.UpsertAsync(code);

        return (otp, code);
    }

    public async Task<bool> VerifyAsync(
        string otp,
        long userId,
        VerificationCodeScenario scenario)
    {
        var verificationCode = await _verificationCodeRepository.GetByIdentifierOrNullAsync(
            identifier: GetCodeIdentifier(userId, scenario),
            scenario: scenario);

        if (verificationCode == null)
        {
            return false;
        }

        var isValid = _hashHelper.Verify(verificationCode.CodeHash, otp);
        if (!isValid)
        {
            verificationCode.IncrementAttemptsCount();
            await _verificationCodeRepository.UpdateAttemptsAsync(verificationCode);
            return false;
        }
        await _verificationCodeRepository.Remove(verificationCode.Identifier, verificationCode.Scenario);

        return true;
    }

    public async Task<VerificationCode?> GetExistedVerificationCodeAsync(
        long userId,
        VerificationCodeScenario scenario)
    {
        return await _verificationCodeRepository.GetByIdentifierOrNullAsync(
            identifier: GetCodeIdentifier(userId, scenario),
            scenario: scenario);
    }

    private string GetCodeIdentifier(long userId, VerificationCodeScenario scenario)
    {
        return scenario switch
        {
            VerificationCodeScenario.VERIFY_EMAIL => $"email-{userId}",
            VerificationCodeScenario.MFA_ENABLE => $"mfa-enable-{userId}",
            _ => $"code-{userId}"
        };
    }

    private string GenerateOtp()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[4];
        rng.GetBytes(bytes);
        int number = BitConverter.ToInt32(bytes, 0) & 0x7FFFFFFF;
        return (number % 900000 + 100000).ToString();
    }
}
