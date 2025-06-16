namespace Messenger.Domain.ValueObjects;

public enum VerificationCodeScenario
{
    VERIFY_EMAIL,
    TOTP_MFA_ENABLE,
    TOTP_MFA_DISABLE,
}
