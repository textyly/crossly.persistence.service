namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<string> Save(Stream dataModelStream);
        Task<Stream?> GetById(string id);
        Task<Stream?> GetByName(string name);
    }
}