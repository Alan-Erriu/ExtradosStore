namespace ExtradosStore.Common.CustomExceptions.PostExceptions
{
    public class PostNotFoundException : Exception
    {
        public PostNotFoundException() : base("the post id not found in data base")
        {

        }

        public PostNotFoundException(string message)
            : base(message)
        {
        }
    }
}
