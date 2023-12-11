namespace ExtradosStore.Common.CustomExceptions.PostExceptions
{
    public class BrandNotFoundException : Exception
    {
        public BrandNotFoundException() : base("The id brand not found")
        {

        }

        public BrandNotFoundException(string message)
            : base(message)
        {
        }
    }
}
