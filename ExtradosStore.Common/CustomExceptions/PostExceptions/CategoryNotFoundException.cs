namespace ExtradosStore.Common.CustomExceptions.PostExceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("The id category not found")
        {

        }

        public CategoryNotFoundException(string message)
            : base(message)
        {
        }
    }
}
