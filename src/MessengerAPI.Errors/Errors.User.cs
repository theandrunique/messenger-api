namespace MessengerAPI.Errors;

public static partial class ApiErrors
{
    public static class User
    {
        public static BaseApiError NotFound(long userId)
            => new BaseApiError(ErrorCode.USER_NOT_FOUND, $"User '{userId}' not found");

        public static BaseApiError NotFound(IEnumerable<long> userIds)
        {
            var userIdsList = userIds.ToList();
            if (userIdsList.Count() == 1)
            {
                NotFound(userIds.First());
            }
            return new BaseApiError(ErrorCode.USER_NOT_FOUND, $"Users '{string.Join(", ", userIdsList)}' not found");
        }
    }
}
