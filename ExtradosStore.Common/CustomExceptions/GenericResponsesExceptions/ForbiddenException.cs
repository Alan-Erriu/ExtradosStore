namespace ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Forbidden")
        {

        }

        public ForbiddenException(string message)
            : base(message)
        {
        }
    }
}
