using ExtradosStore.Common.CustomRequest.PostRequest;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostDAO
    {
        Task<int> DataCreateNewPost(CreateNewPostRequest postRequest);


    }
}