namespace ExtradosStore.Common.CustomExceptions.CarExceptions
{
    public class StockIsLessThanQuantity : Exception
    {
        public StockIsLessThanQuantity() : base("stock is less than quantity")
        {

        }

        public StockIsLessThanQuantity(string message)
            : base(message)
        {
        }
    }
}
