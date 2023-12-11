using Dapper;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Configuration.DBConfiguration;
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

        public async Task<int> DataCreateOffer(CreateOfferRequest offerRequest)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        OfferName = offerRequest.offer_name,
                        OfferDateStart = new DateTimeOffset(offerRequest.offer_date_start).ToUnixTimeMilliseconds(),
                        OfferDateExpiration = new DateTimeOffset(offerRequest.offer_date_expiration).ToUnixTimeMilliseconds(),
                    };


                    return await connection.ExecuteAsync(_sqlInsertNewOffer, parameters);
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
