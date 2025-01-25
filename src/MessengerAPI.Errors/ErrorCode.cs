namespace MessengerAPI.Errors;

public enum ErrorCode
{
    // Auth error codes
    InvalidCredentials = 10001,
    SessionExpired = 10002,
    InvalidToken = 10003,
    InvalidCaptcha = 10004,

    // Attachment error codes,
    AttachmentNotFound = 20001,
    AttachmentTooBig = 20002,
    InvalidUploadFilename = 20003,
    AttachmentNotFoundInObjectStorage = 20004,

    // Channels error codes
    ChannelNotFound = 30002,
    MessageNotFound = 30003,
    MessageToEditNotFound = 30004,
    NotAllowedToInteract = 30005,
    MessageWasSentByAnotherUser = 30006,

    // Users error codes
    UserNotFound = 40001,
    InvalidEmailValidationCode = 40002,

    // Common
    InvalidRequestBody = 50001,
    InvalidJson = 50002,
    InternalServerError = 50003,
}

