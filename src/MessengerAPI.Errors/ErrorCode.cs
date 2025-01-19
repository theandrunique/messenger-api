namespace MessengerAPI.Errors;

public enum ErrorCode
{
    // Auth error codes
    InvalidCredentials = 10001,
    SessionExpired = 10002,
    InvalidToken = 10003,
    InvalidCaptcha = 10004,

    // Files error codes,
    FileTooBig = 20001,
    FileNotFound = 20002,
    InvalidUploadFilename = 20003,

    // Channels error codes
    ChannelNotFound = 30002,
    MessageNotFound = 30003,
    ChannelNotAllowed = 30004,

    // Users error codes
    UserNotFound = 40001,
    InvalidEmailValidationCode = 40002,

    // Common
    InvalidRequestBody = 50001,
    InvalidJson = 50002,
    InternalServerError = 50003,
}

