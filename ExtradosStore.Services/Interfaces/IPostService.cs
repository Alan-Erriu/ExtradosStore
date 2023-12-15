using ExtradosStore.Common.CustomRequest.PostRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostService
    {
        Task<int> CreatePostService(CreateNewPostRequest postRequest);

        Task<int> SetPostStatus(int postId, int userIdFromToken, string nameRole, string newStatus);

        Task<int> SetStatusActiveAndUpdateStock(int postId, int userIdFromToken, string newStatus, int newStock);
        Task<int> UpdatePostService(UpdatePostRequest updateRequest, int userIdFromToken);

    }
}