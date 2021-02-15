namespace QAForum.Application.Common.Models
{
    public static class ErrorMessages
    {
        public static class Account
        {
            public static string GetUserExistMessage => "User already exist.";
            public static string GetUserNotFoundMessage => "User not found.";
            public static string GetUserWrongInfoMessage => "User data is invalid.";
        }

        public static class Entity<T>
        {
            public static string GetNotFoundMessage(long id)
            {
                return $"{typeof(T).Name} with {id} id not found";
            }

            public static string GetNoAccessMessage(long id, string userId)
            {
                return
                    $"You have no access to modify {typeof(T).Name} with {id} id. Request from user with id: {userId}.";
            }
        }
    }
}