namespace ExtradosStore.Common.CustomExceptions
{

    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() : base("The email is already in use.")
        {
        }

        public EmailAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
