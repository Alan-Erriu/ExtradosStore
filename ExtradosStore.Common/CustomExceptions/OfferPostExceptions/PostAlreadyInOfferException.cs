namespace ExtradosStore.Common.CustomExceptions.OfferPostExceptions
{
    public class PostAlreadyInOfferException : Exception
    {
        public PostAlreadyInOfferException() : base("The post is already in an active offer with a not expired expiration date")
        {

        }

        public PostAlreadyInOfferException(string message)
            : base(message)
        {
        }
    }
}
