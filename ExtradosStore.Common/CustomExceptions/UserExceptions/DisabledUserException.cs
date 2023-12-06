namespace ExtradosStore.Common.CustomExceptions.UserExceptions
{
    public class DisabledUserException : Exception
    {
        public DisabledUserException() : base("The user is disabled")
        {

        }

        public DisabledUserException(string message)
            : base(message)
        {
        }
    }
}
