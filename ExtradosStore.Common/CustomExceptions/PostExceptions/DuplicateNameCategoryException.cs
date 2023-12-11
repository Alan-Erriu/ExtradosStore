namespace ExtradosStore.Common.CustomExceptions.PostExceptions
{
    public class DuplicateNameCategoryException : Exception
    {
        public DuplicateNameCategoryException() : base("The name category is already in use")
        {

        }

        public DuplicateNameCategoryException(string message)
            : base(message)
        {
        }
    }
}
