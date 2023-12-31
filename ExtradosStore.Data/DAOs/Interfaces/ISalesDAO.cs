﻿using ExtradosStore.Entities.DTOs.SalesDTO;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface ISalesDAO
    {
        Task<int> DataCreateNewSales(int userId, decimal total);
        Task<List<SaleDTO>> DataGetAllSalesByUserId(int userId);
    }
}