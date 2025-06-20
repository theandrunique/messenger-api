namespace Messenger.ApiErrors;

public enum ErrorCode
{
    // Auth error codes
    AUTH_INVALID_CREDENTIALS = 10001,
    AUTH_SESSION_EXPIRED = 10002,
    AUTH_INVALID_TOKEN = 10003,
    INVALID_CAPTCHA = 10004,
    AUTH_NO_SESSION_INFO_FOUND = 10005,
    AUTH_TOTP_REQUIRED = 10006,
    AUTH_TOTP_MFA_ALREADY_ENABLED = 10007,
    AUTH_EMAIL_CODE_REQUIRED = 10008,
    AUTH_EMAIL_VERIFICATION_REQUIRED = 10009,
    AUTH_INVALID_EMAIL_CODE = 10010,
    AUTH_INVALID_TOTP = 10011,
    AUTH_TOTP_MFA_ALREADY_DISABLED = 10012,
    AUTH_USERNAME_OR_EMAIL_JUST_TAKEN = 10013,

    // Attachment error codes,
    ATTACHMENT_NOT_FOUND = 20001,
    ATTACHMENT_INVALID_UPLOAD_FILENAME = 20003,
    ATTACHMENT_OBJECT_NOT_FOUND = 20004,
    ATTACHMENT_OBJECT_IN_USE = 20005,

    // Channels error codes
    CHANNEL_NOT_FOUND = 30002,
    MESSAGE_NOT_FOUND = 30003,
    CHANNEL_MEMBER_ALREADY_EXISTS = 30004,
    NOT_ALLOWED_TO_INTERACT = 30005,
    MESSAGE_WAS_SEND_BY_ANOTHER_USER = 30006,
    CHANNEL_INVALID_OPERATION_FOR_TYPE = 30007,
    CHANNEL_USER_NOT_MEMBER = 30008,
    CHANNEL_INSUFFICIENT_PERMISSIONS = 30009,
    CHANNEL_NOT_OWNER = 30010,
    MESSAGE_NO_ACKS_FOUND = 30011,

    // Users error codes
    USER_NOT_FOUND = 40001,
    USER_EMAIL_ALREADY_VERIFIED = 40002,

    // Common
    INVALID_REQUEST_BODY = 50001,
    INTERNAL_SERVER_ERROR = 50003,
    UNSUPPORTED_MEDIA_TYPE = 50004,
    INVALID_EMAIL_VERIFICATION_CODE = 50005,
}
