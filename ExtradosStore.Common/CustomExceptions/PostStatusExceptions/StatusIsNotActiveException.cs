namespace ExtradosStore.Common.CustomExceptions.PostStatusExceptions
{
    public class StatusIsNotActiveException : Exception
    {
        public StatusIsNotActiveException() : base("The post status is not active")
        {

        }

        public StatusIsNotActiveException(string message)
            : base(message)
        {
        }
    }
}
