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
        #region querys
        private string _sqlInserIntoOfferPost = @"INSERT INTO [offer_post]  (offer_post_postId,offer_post_offerId,offer_post_discount)      
                                                  VALUES(@OfferPostPostId,@OfferPostOfferId,@OfferPostPostDiscount)";


        private string _sqlSelectAllOfferPost = @"SELECT offer_post_id,offer_post_postId,offer_post_offerId,offer_post_discount FROM [offer_post]";

        private string _sqlSelectOfferId = @"SELECT offer_post_offerId FROM [offer_post] where offer_post_postId = @PostId";

        private string _sqlDeleteOfferPostByPostId = @"DELETE from [offer_post] WHERE offer_post_postId = @PostId";

        private string _sqlDeleteAllOfferPostByOfferId = @"DELETE from [offer_post] WHERE offer_post_offerId = @OfferId";

        private string _SqlSelectDiscountByPostID = @"SELECT offer_post_discount FROM [offer_post] WHERE offer_post_postId = @PostId";
        #endregion


        public async Task<List<OfferPost>> DataGetAllOfferPost()
        {
            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var listOfferPost = (await connection.QueryAsync<OfferPost>(_sqlSelectAllOfferPost)).ToList();
                return listOfferPost;
            }
        }
        public async Task<int> DataGetOfferId(int postId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    PostId = postId
                };
                var offerIdFromDB = await connection.QueryFirstOrDefaultAsync<int>(_sqlSelectOfferId, parameters);
                return offerIdFromDB;
            }

        }
        public async Task<int> DataGerDiscountByPostId(int postId)
        {

            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    PostId = postId
                };
                var offerIdFromDB = await connection.QueryFirstOrDefaultAsync<int>(_SqlSelectDiscountByPostID, parameters);
                return offerIdFromDB;
            }

        }
        public async Task<int> DeleteOfferPostByPostId(int postId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    PostId = postId
                };
                var offerIdFromDB = await connection.ExecuteAsync(_sqlDeleteOfferPostByPostId, parameters);

                return offerIdFromDB;
            }

        }
        public async Task<int> DeleteAllOfferPostByOfferId(int offerId)
        {


            using (var connection = new SqlConnection(_SQLServerConfig.ConnectionStrings))
            {
                var parameters = new
                {
                    OfferID = offerId
                };
                var offerIdFromDB = await connection.ExecuteAsync(_sqlDeleteAllOfferPostByOfferId, parameters);

                return offerIdFromDB;
            }

        }
        public async Task<int> AddPostToOfferData(AddPostToOfferRequest addPostToOfferRequest)
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


    }
}
