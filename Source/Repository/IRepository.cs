namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<string> Save(Stream dataModelStream);
        Task<string[]> GetAll();
        Task<Stream?> GetById(string id);
        Task<Stream?> GetByName(string name);
    }
}