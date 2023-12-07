namespace ExtradosStore.Common.CustomExceptions.JWTExceptions
{
    public class ExpiredRefreshTokenException : Exception
    {
        public ExpiredRefreshTokenException() : base("The refresh token has expired")
        {

        }

        public ExpiredRefreshTokenException(string message)
            : base(message)
        {
        }
    }
}
