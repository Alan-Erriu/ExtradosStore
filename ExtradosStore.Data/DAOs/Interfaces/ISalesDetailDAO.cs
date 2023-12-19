namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ISalesDetailDAO
    {
        Task<int> DataCreateNewSalesDetail(int salesId, int postId, int quantity, decimal price, int discount);
    }
}