namespace Persistence.Repository
{
    public interface IRepository
    {
        Task<string> Save(Stream dataModelStream);
        Task<Stream?> Get(string id);
    }
}