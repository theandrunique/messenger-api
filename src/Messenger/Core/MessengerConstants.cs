namespace Messenger.Core;

public static class MessengerConstants
{
    public static class Attachment
    {
        public const long MaxSize = 10 * 1024 * 1024;
    }

    public static class Message
    {
        public const long MaxAttachmentsCount = 10;
        public const int MaxContentLength = 10000;
    }

    public static class Session
    {
        public const long MaxSessionsCount = 10;
    }

    public static class Cors
    {
        public const string PolicyName = "CorsPolicy";
    }

    public static class Auth
    {
        public const string KeyIdHeaderName = "kid";
        public const string SessionCookieName = "SessionId";
    }

    public static class Images
    {
        public const long MaxSize = 10 * 1024 * 1024;
    }
}
