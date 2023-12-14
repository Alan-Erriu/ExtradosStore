namespace ExtradosStore.Common.CustomExceptions.PostStatusExceptions
{
    public class PostStatusNotFoundException : Exception
    {
        public PostStatusNotFoundException() : base("The id postStatus not found in data base")
        {

        }

        public PostStatusNotFoundException(string message)
            : base(message)
        {
        }

    }
}
