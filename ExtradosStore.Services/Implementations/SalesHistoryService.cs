using ExtradosStore.Common.CustomResponse;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.DTOs.SalesDTO;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class SalesHistoryService : ISalesHistoryService
    {
        ISalesDAO _salesDAO;
        ISalesDetailDAO _salesDetailDAO;
        public SalesHistoryService(ISalesDAO salesDAO, ISalesDetailDAO salesDetailDAO)
        {
            _salesDAO = salesDAO;
            _salesDetailDAO = salesDetailDAO;
        }

        public async Task<List<PurchaseHistoryResponse>> GetHistoryGetUserPurchaseHistory(int userId)
        {
            try
            {
                var listSales = await _salesDAO.DataGetAllSalesByUserId(userId);
                var listSalesDetails = new List<SalesDetailDTO>();

                foreach (var sales in listSales)
                {
                    var details = await _salesDetailDAO.DataGetAllSalesDetailBySalesId(sales.sales_id);
                    listSalesDetails.AddRange(details);
                }


                var result = (from sales in listSales
                              join salesDetails in listSalesDetails on sales.sales_id equals salesDetails.sales_id

                              select new PurchaseHistoryResponse
                              {
                                  post_id = salesDetails.post_id,
                                  quantity = salesDetails.quantity,
                                  total = salesDetails.subtotal,
                                  BuyDate = sales.date_sale,
                              }).ToList();
                return result;

            }
            catch
            {

                throw;
            }
        }
    }
}
