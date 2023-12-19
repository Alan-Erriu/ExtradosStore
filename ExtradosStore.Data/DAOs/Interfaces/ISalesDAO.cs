namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ISalesDAO
    {
        Task<int> DataCreateNewSales(int userId, decimal total);
    }
}