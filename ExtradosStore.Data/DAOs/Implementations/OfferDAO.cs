using Dapper;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class OfferDAO : IOfferDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public OfferDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }

        private string _sqlInsertNewOffer = @"INSERT  INTO [offer]( offer_name, offer_date_start,offer_date_expiration) VALUES
                                              (@OfferName, @OfferDateStart, @OfferDateExpiration)";
        private string _sqlSelectOfferExpirated = "SELECT offer_id FROM [offer] where offer_date_expiration > @DateTimeNow";

        private string _sqlSelectExpirationDateByOfferId = @"SELECT offer_date_expiration FROM [offer] WHERE offer_id =@OfferID";

        private string _sqlSelectAllOffer = @"SELECT offer_id,offer_name, offer_date_start,offer_date_expiration FROM [offer] ";
        public async Task<int> DataCreateOffer(CreateOfferRequest offerRequest)
        {
            DateTimeOffset offerExpirationDate = offerRequest.offer_date_expiration.Date.Add(new TimeSpan(12, 0, 0));
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        OfferName = offerRequest.offer_name,
                        OfferDateStart = new DateTimeOffset(offerRequest.offer_date_start).ToUnixTimeMilliseconds(),
                        OfferDateExpiration = offerExpirationDate.ToUnixTimeMilliseconds(),
                    };






                    return await connection.ExecuteAsync(_sqlInsertNewOffer, parameters);
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<long> DataGetExpirationDateByOfferId(int offerID)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        OfferID = offerID
                    };
                    var expirationDate = await connection.QueryFirstOrDefaultAsync<long>(_sqlSelectExpirationDateByOfferId, parameters);
                    return expirationDate;
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<List<Offer>> GetAllOffer()
        {
            try
            {
                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var listOffer = (await connection.QueryAsync<Offer>(_sqlSelectAllOffer)).ToList();
                    return listOffer;
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<List<int>> GetOfferExpiratedData()
        {
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    DateTimeNow = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                };
                var offerlistIdExpired = (await connection.QueryAsync<int>(_sqlSelectOfferExpirated, parameters)).ToList();
                return offerlistIdExpired;
            };
        }
    }
}
