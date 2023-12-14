using Dapper;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Configuration.DBConfiguration;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Entities.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace ExtradosStore.Data.DAOs.Implementations
{
    public class OfferPostDAO : IOfferPostDAO
    {
        private SQLServerConfig _SQLServerConfig;
        public OfferPostDAO(IOptions<SQLServerConfig> bdConfig)
        {
            _SQLServerConfig = bdConfig.Value;

        }

        private string _sqlInserIntoOfferPost = @"INSERT INTO [offer_post]  (offer_post_postId,offer_post_offerId,offer_post_discount)      
                                                  VALUES(@OfferPostPostId,@OfferPostOfferId,@OfferPostPostDiscount)";
        private string _sqlDeleteOfferPostExpirated = @"DELETE FROM [offer_post] WHERE offer_post_offerId = @OfferId";

        private string _sqlSelectAllOfferPost = @"SELECT offer_post_id,offer_post_postId,offer_post_offerId,offer_post_discount FROM [offer_post]";



        public async Task<List<OfferPost>> DataGetAllOfferPost()
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var listOfferPost = (await connection.QueryAsync<OfferPost>(_sqlSelectAllOfferPost)).ToList();
                    return listOfferPost;
                }
            }
            catch
            {

                throw;
            }
        }
        public async Task<int> AddPostToOfferData(AddPostToOfferRequest addPostToOfferRequest)
        {
            try
            {

                using (var connnection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new
                    {
                        OfferPostPostId = addPostToOfferRequest.offer_post_postId,
                        OfferPostOfferId = addPostToOfferRequest.offer_post_offerId,
                        OfferPostPostDiscount = addPostToOfferRequest.offer_post_discount
                    };
                    var rowsAffected = await connnection.ExecuteAsync(_sqlInserIntoOfferPost, parameters);
                    return rowsAffected;
                }
            }
            catch
            {

                throw;
            }
        }

        public async Task<int> DeleteOfferPostExpirated(int offerIdRequest)
        {
            try
            {

                using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
                {
                    var parameters = new { OfferId = offerIdRequest };
                    var rowsAffected = await connection.ExecuteAsync(_sqlDeleteOfferPostExpirated, parameters);
                    return rowsAffected;
                }
            }
            catch
            {

                throw;
            }
        }
    }
}
