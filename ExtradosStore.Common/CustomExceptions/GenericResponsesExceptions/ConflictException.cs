namespace ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base("Conflict")
        {

        }

        public ConflictException(string message)
            : base(message)
        {
        }
    }
}
