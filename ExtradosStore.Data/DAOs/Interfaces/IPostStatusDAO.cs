using ExtradosStore.Entities.Models;

namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostStatusDAO
    {
        Task<int> DataGetPostStatusIdByName(string statusName);
        Task<List<PostStatus>> DataGetAllPostStatus();
    }
}