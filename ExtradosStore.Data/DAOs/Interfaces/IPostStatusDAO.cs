namespace ExtradosStore.Data.DAOs.Interfaces
{
    public interface IPostStatusDAO
    {
        Task<int> DataGetPostStatusIdByName(string statusName);
    }
}