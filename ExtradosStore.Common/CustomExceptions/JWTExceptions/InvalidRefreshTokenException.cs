namespace ExtradosStore.Common.CustomExceptions.JWTExceptions
{
    public class InvalidRefreshTokenException : Exception
    {
        public InvalidRefreshTokenException() : base("The provided refresh token does not match the one stored in the database")
        {

        }

        public InvalidRefreshTokenException(string message)
            : base(message)
        {
        }
    }
}
