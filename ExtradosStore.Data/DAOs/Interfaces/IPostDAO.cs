using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Common.CustomRequest.PostSearchRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostDAO
    {
        Task<int> DataCreateNewPost(CreateNewPostRequest postRequest);
        Task<int> DataSetStatusActiveToPaused(int statusId, int postId);
        Task<int> DataGetUserIdByPostId(int postId);
        Task<int> DataUpdateStockAndSetStatusActive(int postId, int statusId, int newStock);
        Task<int> DataUpdatePost(UpdatePostRequest updateRequest);
        Task<int> DataGetStatusIdByPostId(int postId);
        Task<List<PostWithOfferDTO>> SearchPostActive(PostSearchRequest postSearchRequest, int statusActiveId);
        Task<StockAndStatusDTO> DataGetStatusAndStockByPostId(int postId);
        Task<PostPriceImgAndName> DataGetPostPriceNameAndImgById(int postId);
        Task<List<PostWithOfferDTO>> GetAllPostActive(int statusActiveId);
        Task<PostWithOfferDTO> DataGetPostByPostId(int postId, int statusActiveId);

        Task<List<PostDTO>> DataAllPostByUserId(int userId);

        Task<List<PostWithOfferDTO>> GetAllPostActiveWithOffer(int statusActiveId, long currentTimeEpoch);


    }
}