using ExtradosStore.Common.CustomResponse;

namespace ExtradosStore.Services.Interfaces
{
    public interface ISalesHistoryService
    {
        Task<List<PurchaseHistoryResponse>> GetHistoryGetUserPurchaseHistory(int userId);
    }
}