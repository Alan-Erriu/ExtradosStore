using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostService
    {
        Task<int> CreatePostService(CreateNewPostRequest postRequest);

        Task<List<PostActiveDTO>> GetAllPostActiveService();

        Task<int> SetStatusActiveToPaused(int postId, int userIdFromToken, string nameRole, string newStatus);
    }
}