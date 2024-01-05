namespace ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions
{

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized")
        {

        }

        public UnauthorizedException(string message)
            : base(message)
        {
        }
    }
}
