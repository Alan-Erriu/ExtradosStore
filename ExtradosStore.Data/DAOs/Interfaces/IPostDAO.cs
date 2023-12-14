using ExtradosStore.Common.CustomRequest.PostRequest;
using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostDAO
    {
        Task<int> DataCreateNewPost(CreateNewPostRequest postRequest);
        Task<List<Post>> DataGetAllPostActive();

    }
}