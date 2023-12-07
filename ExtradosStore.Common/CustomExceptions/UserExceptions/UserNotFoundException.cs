namespace ExtradosStore.Common.CustomExceptions.UserExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException() : base("User Not Found")
        {

        }

        public UserNotFoundException(string message)
            : base(message)
        {
        }
    }
}
