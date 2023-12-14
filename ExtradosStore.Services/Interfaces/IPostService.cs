using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Entities.DTOs.PostDTOs;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostService
    {
        Task<int> CreatePostService(CreateNewPostRequest postRequest);
        Task<List<Post>> GetAllPostActiveWithOffer();
        Task<List<PostActiveDTO>> GetAllPostActiveService2();
    }
}