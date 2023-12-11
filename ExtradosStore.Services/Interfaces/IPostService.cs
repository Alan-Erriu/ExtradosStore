using ExtradosStore.Common.CustomRequest.PostRequest;

namespace ExtradosStore.Services.Interfaces
{
    public interface IPostService
    {
        Task<int> CreatePostService(CreateNewPostRequest postRequest);
    }
}