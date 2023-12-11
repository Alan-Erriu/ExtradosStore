namespace ExtradosStore.Common.CustomExceptions.PostExceptions
{
    public class DuplicateNameBrandException : Exception
    {
        public DuplicateNameBrandException() : base("The name brand is already in use")
        {

        }

        public DuplicateNameBrandException(string message)
            : base(message)
        {
        }
    }
}
