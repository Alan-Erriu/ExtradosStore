namespace ExtradosStore.Common.CustomExceptions.UserExceptions
{
    public class UserNotFoundExceptioncs : Exception
    {
        public UserNotFoundExceptioncs() : base("User Not Found")
        {

        }

        public UserNotFoundExceptioncs(string message)
            : base(message)
        {
        }
    }
}
