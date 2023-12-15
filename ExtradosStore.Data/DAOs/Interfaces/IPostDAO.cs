using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostDAO
    {
        Task<int> DataCreateNewPost(CreateNewPostRequest postRequest);
        Task<List<Post>> DataGetAllPostActive();
        Task<int> DataSetStatusActiveToPaused(int statusId, int postId);
        Task<int> DataGetUserIdByPostId(int postId);
        Task<int> DataUpdateStockAndSetStatusActive(int postId, int statusId, int newStock);
        Task<int> DataUpdatePost(UpdatePostRequest updateRequest);
        Task<int> DataGetStatusIdByPostId(int postId);


    }
}