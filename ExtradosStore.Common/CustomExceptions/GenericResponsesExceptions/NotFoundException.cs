namespace ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found")
        {

        }

        public NotFoundException(string message)
            : base(message)
        {
        }
    }
}
