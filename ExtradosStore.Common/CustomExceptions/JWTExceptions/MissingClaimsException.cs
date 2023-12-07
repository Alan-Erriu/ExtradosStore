namespace ExtradosStore.Common.CustomExceptions.JWTExceptions
{
    public class MissingClaimsException : Exception
    {
        public MissingClaimsException() : base("token claims not found")
        {

        }

        public MissingClaimsException(string message)
            : base(message)
        {
        }
    }
}
