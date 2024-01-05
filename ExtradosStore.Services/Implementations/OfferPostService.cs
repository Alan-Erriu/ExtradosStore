using ExtradosStore.Common.CustomExceptions.GenericResponsesExceptions;
using ExtradosStore.Common.CustomRequest.OfferRequest;
using ExtradosStore.Data.DAOs.Interfaces;
using ExtradosStore.Services.Interfaces;

namespace ExtradosStore.Services.Implementations
{
    public class OfferPostService : IOfferPostService
    {
        IOfferPostDAO _offerPostDAO;
        IOfferDAO _offerDAO;
        IPostDAO _postDAO;
        IPostStatusDAO _postStatusDAO;
        public OfferPostService(IOfferPostDAO offerPostDAO, IOfferDAO offerDAO, IPostDAO postDAO, IPostStatusDAO postStatusDAO)
        {
            _offerPostDAO = offerPostDAO;
            _offerDAO = offerDAO;
            _postDAO = postDAO;
            _postStatusDAO = postStatusDAO;


        }

        public async Task<int> AddPostToOfferService(AddPostToOfferRequest addPostToOfferRequest, int userIdFromToken)
        {

            var postUserIdFromDB = await _postDAO.DataGetUserIdByPostId(addPostToOfferRequest.offer_post_postId);
            if (userIdFromToken != postUserIdFromDB)
            {
                throw new UnauthorizedException("the user id of the token is different from that of the publication");
            }
            var postStatusIdFromBack = await _postDAO.DataGetStatusIdByPostId(addPostToOfferRequest.offer_post_postId);
            var statusActiveId = await _postStatusDAO.DataGetPostStatusIdByName("active");
            if (postStatusIdFromBack != statusActiveId) throw new BadRequestException("Only active posts can be added to offers");
            var offerIDFromDb = await _offerPostDAO.DataGetOfferId(addPostToOfferRequest.offer_post_postId);
            var currentEpochTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long expirationDateFromOfferID = await _offerDAO.DataGetExpirationDateByOfferId(offerIDFromDb);
            //si es 0 post nunca tuvo oferta, caso contrario tiene vecha de vencimiento, esta debe estar expirada para ser agregada a otra oferta
            if (expirationDateFromOfferID != 0 && expirationDateFromOfferID > currentEpochTime) throw new ConflictException("This publication already belongs to an active offer");
            await _offerPostDAO.DeleteOfferPostByPostId(addPostToOfferRequest.offer_post_postId);
            var rowsAffected = await _offerPostDAO.AddPostToOfferData(addPostToOfferRequest);
            return rowsAffected;
        }


        public async Task<int> DeleteOfferPostByPostId(int postId, int userIdFromToken, string nameRole)
        {

            var postUserIdFromDB = await _postDAO.DataGetUserIdByPostId(postId);
            if (userIdFromToken != postUserIdFromDB && nameRole == "user")
            {
                throw new UnauthorizedException("the user id of the token is different from that of the publication");
            }
            var rowsAffected = await _offerPostDAO.DeleteOfferPostByPostId(postId);
            if (rowsAffected == 0) throw new NotFoundException("offer post not found");
            return rowsAffected;

        }
        public async Task<int> DeleteAllOfferPostByOfferId(int offerId)
        {
            var rowsAffected = await _offerPostDAO.DeleteAllOfferPostByOfferId(offerId);
            return rowsAffected;

        }

    }
}
